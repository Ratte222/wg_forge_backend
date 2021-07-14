﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using wg_forge_backend.JWT;
using BLL.Interfaces;
using BLL.DTO;
using BLL.Infrastructure;
using wg_forge_backend.Models;

//статьи с реализацией авторизвции JWT https://fuse8.ru/articles/using-asp-net-core-identity-and-jwt,
//https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api,
//https://metanit.com/sharp/aspnet5/23.7.php
namespace wg_forge_backend.Controllers
{

    
    [ApiController]
    public class AccountController:Controller
    {
        private IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }

        /// <summary>
        /// Generates a token for registered users 
        /// </summary>
        /// <param name="loginModelDTO"></param>
        /// <returns></returns>
        [HttpPost("/authenticate")]
        [ProducesResponseType(typeof(List<LoginResponseModel>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        //[ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Authenticate(LoginModelDTO loginModelDTO)
        {
            var identity = GetIdentity(loginModelDTO);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new LoginResponseModel()
            {
                AccessToken = encodedJwt,
                UserName = identity.Name
            };

            return Json(response);
        }


        /// <summary>
        /// Registers new users 
        /// </summary>
        /// <param name="registerModelDTO"></param>
        /// <returns></returns>
        [HttpPost("/registration")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        //у меня для этого запроса с одним и тем же кодом ответа может прийтиразные тела ответа
        // это нужно исправлять?
        //[ProducesResponseType(typeof(ValidationException), 400)]        
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Registration(RegisterModelDTO registerModelDTO)
        {
            _account.Registration(registerModelDTO);
            return StatusCode(200, "Register succes");
        }

        private ClaimsIdentity GetIdentity(LoginModelDTO loginModelDTO)
        {
            AccountModelDTO accountModelDTO = _account.Authenticate(loginModelDTO);
            if (accountModelDTO != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, accountModelDTO.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, accountModelDTO.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}