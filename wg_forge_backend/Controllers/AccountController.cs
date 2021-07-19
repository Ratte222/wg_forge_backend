using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using BLL.DTO;

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
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<CatOwner> userManager, SignInManager<CatOwner> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
    }
}
