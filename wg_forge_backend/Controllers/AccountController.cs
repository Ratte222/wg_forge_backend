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
using wg_forge_backend.Models;

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

        /// <summary>
        /// Registers new users 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        //[ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        [ProducesResponseType(typeof(string), 500)]        
        public async Task<IActionResult> Register(RegisterModelDTO model)
        {
            CatOwner catOwner = new CatOwner { Email = model.Email, UserName = model.UserName, Age = model.Age, CatPoints = 0};
            // add new user
            var result = await _userManager.CreateAsync(catOwner, model.Password);
            if (result.Succeeded)
            {
                try
                {
                    // set cokies
                    //await _signInManager.SignInAsync(catOwner, false);
                    var allRoles = _roleManager.Roles.ToList();
                    await _userManager.AddToRoleAsync(catOwner, AccountRole.User);
                    string jwt = CreateJWT(GetIdentity(new List<Claim>() {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, catOwner.Id)
                    //new Claim(ClaimsIdentity.DefaultIssuer, catOwner.Email)
                }, "ConfirmMail"));
                    _emailService.SendConfirmationEmail(catOwner.Email, "Confirm your registration",
                        $"https://localhost:5001/Account/ConfirmEmail?t={jwt}");
                    return StatusCode(200, "Registration succsess");
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result.Errors);                
            }            
        }

        /// <summary>
        /// Confirm email
        /// </summary>
        /// <param name="t">confirm mail token</param>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //[ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> ConfirmEmail(string t)
        {
            ClaimsPrincipal claimsPrincipal; 
            if (ValidationJWT(t, out claimsPrincipal))
            {
                CatOwner catOwner = await _userManager.FindByIdAsync(claimsPrincipal.Identity.Name);
                catOwner.EmailConfirmed = true;
                var result = _userManager.UpdateAsync(catOwner);
                if (result.Result.Succeeded)
                    return StatusCode(200, "Email confitm");
                else
                    return BadRequest(result.Result.Errors);
            }                
            return BadRequest("Invalid token");
        }
        /// <summary>
        /// Generates a token for registered users 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<LoginResponseModel>), 200)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]        
        public async Task<IActionResult> Login(LoginModelDTO model)
        {           
            //var result =
                //await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            CatOwner catOwner = await _userManager.FindByNameAsync(model.UserName);
            if (await _userManager.CheckPasswordAsync(
                catOwner, model.Password))
            {
                if (!catOwner.EmailConfirmed)
                    return BadRequest("Please, confirm e-mail");
                LoginResponseModel loginResponseModel = new LoginResponseModel()
                {
                    AccessToken = CreateJWT(GetIdentity(new List<Claim>() {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, model.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, AccountRole.CatOwner)
                    })),
                    UserName = model.UserName
                };
                return Json(loginResponseModel);
            }
            else
            {
                return this.BadRequest("Wrong password or userName ");
            }           
        }

        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    // удаляем аутентификационные куки
        //    await _signInManager.SignOutAsync();
        //    return StatusCode(200, "Logout done");
        //}

        private bool ValidationJWT(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            var mySecurityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();            
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
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

        private string CreateJWT(ClaimsIdentity claimsIdentity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
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
        private ClaimsIdentity GetIdentity(List<Claim> claims, string authenticationType = "Token")
        {
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
