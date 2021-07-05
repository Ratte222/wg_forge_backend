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
        private ILogger<CatsController> logger;
        public CatsController(ITaskService taskService_, ILogger<CatsController> logger_)
        {
            taskService = taskService_;
            logger = logger_;
        }
        [Route("cats/")]
        public IActionResult Cats(string attribute, string order, int? offset, int? limit
            //string? attribute = "color", string? order
            /*= "asc", int? offset = 5, int? limit = 2*/)
        {
             return Json(taskService.GetCats(attribute, order, offset, limit));
            
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
        [Route("cat/"), HttpPost]
        public async Task<IActionResult> AddNewCat(NewCatDTO newCatDTO)
        {
            taskService.AddCat(newCatDTO);
            return StatusCode(200);
        }
    }
}
