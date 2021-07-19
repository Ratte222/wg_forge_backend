using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using BLL.DTO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using DAL.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace wg_forge_backend.Controllers
{

    //https://docs.microsoft.com/ru-ru/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio
    //https://metanit.com/sharp/aspnet5/16.2.php
    
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController:Controller
    {
        private readonly UserManager<CatOwner> _userManager;
        private readonly SignInManager<CatOwner> _signInManager;
        private AppSettings _appSettings;
        private EmailService _emailService;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<CatOwner> userManager, SignInManager<CatOwner> signInManager,
            RoleManager<IdentityRole> roleManager, IOptions<AppSettings> appSettings, IOptions<EmailService> emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _emailService = emailService.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModelDTO model)
        {
            CatOwner catOwner = new CatOwner { Email = model.Email, UserName = model.UserName, Age = model.Age, CatPoints = 0};
            // add new user
            var result = await _userManager.CreateAsync(catOwner, model.Password);
            if (result.Succeeded)
            {
                // set cokies
                await _signInManager.SignInAsync(catOwner, false);
                var allRoles = _roleManager.Roles.ToList();
                await _userManager.AddToRoleAsync(catOwner, 
                    _roleManager.KeyNormalizer.NormalizeName(AccountRole.CatOwner));
                string body = $"<h2>Confirm mail</h2>" +
                    //$"<p><a href=\"https://localhost:5001/Account/ConfirmEmail?t={CreateJWT(GetIdentity(catOwner.UserName))}\">" +
                    $"<p><a href=\"https://localhost:5001/Account/ConfirmEmail?t={CreateJWT()}\">" +
                    $"Click here</a></p> ";
                _emailService.SendEmail(catOwner.Email, "CatService", body);
                return StatusCode(200, "Registration succsess");
            }
            else
            {
                return BadRequest(result.Errors);
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }            
        }

        public IActionResult ConfirmEmail(string t)
        {
            if (ValidationJWT(t))
                return StatusCode(200, "Email confitm");
            else
                return BadRequest("Invalid token");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModelDTO model)
        {           
            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return StatusCode(200);
            }
            else
            {
                return this.BadRequest("Wrong password or userName ");
            }           
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return StatusCode(200, "Logout done");
        }

        private bool ValidationJWT(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _appSettings.Issuer,
                    ValidAudience = _appSettings.Audience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string CreateJWT(/*ClaimsIdentity claimsIdentity*/)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Audience,
                notBefore: now,
                //claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(_appSettings.Lifetime)),
                signingCredentials: new SigningCredentials(new 
                SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret)), 
                SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(string username)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
