using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Infrastructure;
using Microsoft.Extensions.Logging;
namespace wg_forge_backend.Controllers
{
    
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
            try
            { return Json(taskService.GetCats(attribute, order, offset, limit)); }
            catch(ValidationException ex)
            {
                //return this.Problem(ex.Message);
                return this.BadRequest(ex.Message);
            }
            catch(SelectException ex)
            {
                return this.Problem(ex.Message);
            }
            catch(Exception ex)
            {
                logger.LogWarning($"{ex.Message}\r\n {ex?.StackTrace}");
                return this.StatusCode(502);
            }
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
    }
}
