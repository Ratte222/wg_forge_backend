using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Infrastructure;
using Microsoft.Extensions.Logging;
using System.IO;
using BLL.DTO;
using System.Text.Json;

namespace wg_forge_backend.Controllers
{
    [ApiController]
    public class CatsController : Controller
    {
        private ITaskService taskService;
        public CatsController(ITaskService taskService_)
        {
            taskService = taskService_;
        }
        //если стоит атрибут ApiController и в аргументах объект класса у меня ошибка 415

        /// <summary>
        /// Get a list of all cats
        /// </summary>
        /// <param name="attribute">By which field to sort</param>
        /// <param name="order">asc/desc</param>
        /// <param name="offset">offset</param>
        /// <param name="limit">sampling limit </param>
        /// <returns>JSON</returns>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't return list cats right now</response>
        [HttpGet("cats/")]
        [ProducesResponseType(typeof(List<CatDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Cats(/*QueryCatColorInfoDTO queryCatColorInfoDTO*/
            string attribute, string order, int? offset, int? limit
            //string? attribute = "color", string? order
            /*= "asc", int? offset = 5, int? limit = 2*/)
        {
            List<CatDTO> catDTO = taskService.GetCats(attribute, order, offset, limit
                 /*queryCatColorInfoDTO.Attribute, queryCatColorInfoDTO.Order,
                 //queryCatColorInfoDTO.Offset, queryCatColorInfoDTO.Limit*/);
            return Json(catDTO);
            
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns>text</returns>
        [HttpGet("ping/")]
        public IActionResult Ping()
        {
            return Content(taskService.Ping());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ex_1/")]//проборосил сюда для наглядности
        public IActionResult Exercise1()
        {
            return Json(taskService.Exercise1());
        }

        [HttpGet("ex_2/")]//проборосил сюда для наглядности
        public IActionResult Exercise2()
        {
            return Json(taskService.Exercise2());
        }

        /// <summary>
        /// Adds a new cat 
        /// </summary>
        /// <param name="newCatDTO">Cat created</param>
        /// <response code="200">Successfully added a new cat</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't create cat right now</response>
        [HttpPost("cat/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddNewCat(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.AddCat(newCatDTO);
            return StatusCode(200, "Successfully added a new cat");
        }

        //[HttpPost("cat/Edit/")]
        //public IActionResult CatEdit(NewCatDTO newCatDTO)
        //{
        //    if (!ModelState.IsValid)//added for passing tests
        //        return BadRequest();
        //    taskService.EditCat(newCatDTO);
        //    return StatusCode(200);
        //}

        /// <summary>
        /// Change the record of the cat
        /// </summary>
        /// <param name="newCatDTO">New data, name cannot be changed</param>
        /// <response code="200">Cahange sucsess update</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't edit cat right now</response>
        [HttpPut("cat/Edit/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatEdit(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.EditCat(newCatDTO);
            return StatusCode(200, "Cahange sucsess update");
        }

        //[HttpPost("cat/Delete/")]
        //[HttpDelete("cat/Delete/")]
        /// <summary>
        /// Deletes the entry of the cat by name 
        /// </summary>
        /// <param name="catDTO">The name of the cat to be deleted</param>
        /// <response code="200">Cat deleted</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't delete cat right now</response>
        /// <example>cat/Delete?Name=Chlo</example>
        [HttpPut("cat/Delete/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatDelete(CatDTO catDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.DeleteCat(catDTO);
            //taskService.DeleteCat(new CatDTO {Name = Name });
            return StatusCode(200, "Cat deleted");
        }

        /// <summary>
        /// Returns all owners and their cats
        /// </summary>
        /// <returns>json</returns>
        /// <response code="500">Oops! Can't return list cats right now</response>
        [HttpGet("catOwners")]
        [ProducesResponseType(typeof(List<CatOwnerDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatOwners()
        {
            return Json(taskService.GetCatOwners());
        }
    }
}
