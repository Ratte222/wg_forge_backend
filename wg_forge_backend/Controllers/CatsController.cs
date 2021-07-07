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
        [Route("cats/")]
        public IActionResult Cats(/*QueryCatColorInfoDTO queryCatColorInfoDTO*/
            string attribute, string order, int? offset, int? limit
            //string? attribute = "color", string? order
            /*= "asc", int? offset = 5, int? limit = 2*/)
        {
            return Json(taskService.GetCats(attribute, order, offset, limit
                 /*queryCatColorInfoDTO.Attribute, queryCatColorInfoDTO.Order,
                 //queryCatColorInfoDTO.Offset, queryCatColorInfoDTO.Limit*/));
            
        }
        [Route("ping/")]
        public IActionResult Ping()
        {
            return Content(taskService.Ping());
        }

        [Route("ex_1/")]//проборосил сюда для наглядности
        public IActionResult Exercise1()
        {
            return Json(taskService.Exercise1());
        }
        [Route("ex_2/")]//проборосил сюда для наглядности
        public IActionResult Exercise2()
        {
            return Json(taskService.Exercise2());
        }
        //если не стоит атрибут ApiController у меня класс не заполняется

        [Route("cat/"), HttpPost]
        public IActionResult AddNewCat(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.AddCat(newCatDTO);
            return StatusCode(200);
        }

        [Route("cat/Edit/"), HttpPost]
        public IActionResult CatEdit(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.EditCat(newCatDTO);
            return StatusCode(200);
        }

        [Route("cat/Delete/"), HttpPost]
        public IActionResult CatDelete(CatDTO catDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            taskService.DeleteCat(catDTO);
            return StatusCode(200);
        }
    }
}
