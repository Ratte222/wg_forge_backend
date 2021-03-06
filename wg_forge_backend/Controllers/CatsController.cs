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
using wg_forge_backend.Models;
using System.Text.Json;
using DAL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using DAL.Entities;

namespace wg_forge_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private ITaskService _taskService;
        AppSettings _appSettings;
        private readonly IWebHostEnvironment _environment;
        public CatsController(ITaskService taskService_, IOptions<AppSettings> appSettings,
            IWebHostEnvironment IHostingEnvironment)
        {
            _taskService = taskService_;
            _appSettings = appSettings.Value;
            _environment = IHostingEnvironment;
        }
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
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("all_cats/")]
        [ProducesResponseType(typeof(List<CatDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AllCats(/*QueryCatColorInfoDTO queryCatColorInfoDTO*/
            string attribute, string order, int? offset, int? limit
            //string? attribute = "color", string? order
            /*= "asc", int? offset = 5, int? limit = 2*/)
        {
            List<CatDTO> catDTO = _taskService.GetAllCats(attribute, order, offset, limit
                 /*queryCatColorInfoDTO.Attribute, queryCatColorInfoDTO.Order,
                 //queryCatColorInfoDTO.Offset, queryCatColorInfoDTO.Limit*/);
            return Ok(catDTO);
            
        }

        /// <summary>
        /// Get a list of all cats
        /// </summary>
        /// <returns>JSON</returns>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Oops! Can't return list cats right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("cats/")]
        [ProducesResponseType(typeof(List<CatDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Cats()
        {
            return Ok(_taskService.GetCats(this.User.Identity.Name));
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns>text</returns>
        [HttpGet("ping/")]
        public IActionResult Ping()
        {
            return Content(_taskService.Ping());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ex_1/")]//проборосил сюда для наглядности
        [ProducesResponseType(typeof(List<CatColorInfoDTO>), 200)]
        public IActionResult Exercise1()
        {
            return Ok(_taskService.Exercise1());
        }

        [HttpGet("ex_2/")]//проборосил сюда для наглядности
        [ProducesResponseType(typeof(List<CatStatDTO>), 200)]
        public IActionResult Exercise2()
        {
            return Ok(_taskService.Exercise2());
        }

        /// <summary>
        /// Get cat colors
        /// </summary>
        /// <response code="200">JSON</response>
        /// <response code="500">Oops! Can't get cat colors right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("cat/")]
        [ProducesResponseType(typeof(AddOrEditCatResponseModel), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddNewCat()
        {
            AddOrEditCatResponseModel addOrEditCatResponseModel = new AddOrEditCatResponseModel();
            addOrEditCatResponseModel.HexColor = _appSettings.HexColor;
            addOrEditCatResponseModel.ReasoneAddCat = _appSettings.ReasoneDeleteCat.Where(i => i.Value > 0)
                .ToDictionary(i => i.Key, j => j.Value);
            return Ok(addOrEditCatResponseModel);
        }

        /// <summary>
        /// Adds a new cat 
        /// </summary>
        /// <param name="newCatDTO">Cat created</param>
        /// <response code="200">Successfully added a new cat</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't create cat right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpPost("cat/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddNewCat(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            _taskService.AddCat(newCatDTO, this.User.Identity.Name);
            return StatusCode(200, "Successfully added a new cat");
        }

        /// <summary>
        /// Get cat colors
        /// </summary>
        /// <response code="200">JSON</response>
        /// <response code="500">Oops! Can't get cat colors right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("cat/Edit/")]
        [ProducesResponseType(typeof(AddOrEditCatResponseModel), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatEdit()
        {            
            return Ok(_appSettings.HexColor);
        }

        /// <summary>
        /// Change the record of the cat
        /// </summary>
        /// <param name="newCatDTO">New data, name cannot be changed</param>
        /// <response code="200">Cahange sucsess update</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="500">Oops! Can't edit cat right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpPut("cat/Edit/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatEdit(NewCatDTO newCatDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            _taskService.EditCat(newCatDTO, this.User.Identity.Name);
            return StatusCode(200, "Cahange sucsess update");
        }

        /// <summary>
        /// Get reasone delete cat
        /// </summary>
        /// <response code="200">JSON</response>
        /// <response code="500">Oops! Can't get cat colors right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("cat/Delete/")]
        [ProducesResponseType(typeof(Dictionary<string, int>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatDelete()
        {
            return Ok(_appSettings.ReasoneDeleteCat.Where(i=>i.Value<=0).ToDictionary(i=>i.Key, j=>j.Value));
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
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpPut("cat/Delete/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatDelete(CatDTO catDTO)
        {
            if (!ModelState.IsValid)//added for passing tests
                return BadRequest();
            _taskService.DeleteCat(catDTO, this.User.Identity.Name);
            //taskService.DeleteCat(new CatDTO {Name = Name });
            return StatusCode(200, "Cat deleted");
        }

        /// <summary>
        /// Returns all owners and their cats
        /// </summary>
        /// <returns>json</returns>
        /// <response code="500">Oops! Can't return list cats right now</response>
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("catOwners")]
        [ProducesResponseType(typeof(List<CatOwnerDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatOwners()
        {
            return Ok(_taskService.GetCatOwners());
        }

        /// <summary>
        /// Returns info about authorized owner and their cats
        /// </summary>
        /// <returns>json</returns>
        /// <response code="500">Oops! Can't return list cats right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("catOwner")]
        [ProducesResponseType(typeof(CatOwnerDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CatOwner()
        {            
            return Ok(_taskService.GetCatOwner(this.User.Identity.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Oops! Can't added cat photo right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpPost("addCatPhoto")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddCatPhoto([FromForm]IFormFileCollection files, [FromQuery] string catName,
            string path/*, [FromBody] string catOwnerLogin*/)
        {
            string catOwnerUserName = "Artur7";
            _taskService.CheckCatInOwner(catName, this.User.Identity.Name);
            List<CatPhotoDTO> catPhotoDTO = new List<CatPhotoDTO>();
            foreach (var file in files)
            {
                string newFileName = SaveFiles(file, path);
                if (!String.IsNullOrEmpty(newFileName))
                    catPhotoDTO.Add(new CatPhotoDTO() { CatPhotoName = newFileName });
            }
            _taskService.AddCatPhoto(catPhotoDTO, catName);
            return StatusCode(200);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Oops! Can't added cat photo right now</response>
        [Authorize(Roles = AccountRole.CatOwner)]
        [HttpGet("getCatPhoto")]
        [ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(ValidationException), 400)]
        [ProducesResponseType(typeof(string), 500)]        
        public IActionResult GetCatPhoto(string path)
        {
            _taskService.CheckPhotoExistInCat(this.User.Identity.Name, path);
            string contentType = $"image/{Path.GetExtension(path)}";
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), 
                defaultPathForCatPhoto, path);
            if (!System.IO.File.Exists(fullPath))
                return BadRequest("File not exist");
            //StreamReader streamReader = new StreamReader(path);
            return File(new FileStream(fullPath, FileMode.Open), contentType, Path.GetFileName(path));
        }

        private string defaultPathForCatPhoto = "CatPhoto";
        private string SaveFiles(IFormFile file, string path)
        {
            var fileName = string.Empty;
            string PathDB = string.Empty;
            string newFileName = string.Empty;
            if (file.Length > 0)
            {
                //Getting FileName
                fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                //Assigning Unique Filename (Guid)
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                //Getting file Extension
                var FileExtension = Path.GetExtension(fileName);

                // concating  FileName + FileExtension
                newFileName = myUniqueFileName + FileExtension;

                // Combines two strings into a path.
                string partialPath = Path.Combine(Directory.GetCurrentDirectory(), defaultPathForCatPhoto, path);
                fileName = Path.Combine(partialPath, newFileName);
                if (!Directory.Exists(partialPath))
                    Directory.CreateDirectory(partialPath);
                // if you want to store path of folder in database
                PathDB = Path.Combine(path, newFileName);

                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return PathDB;
        }
    }
}
