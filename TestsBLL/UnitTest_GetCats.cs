using NUnit.Framework;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using BLL.Infrastructure;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestsBLL
{
    public class Tests
    {
        private ITaskService taskService;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder<CatContext> options = new DbContextOptionsBuilder<CatContext>();
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wg_forge_backend;Trusted_Connection=True;MultipleActiveResultSets=true");
            CatContext catContext = new CatContext(options.Options);
            taskService = new TaskServices(catContext);
        }

        [Test]
        public void TestGetCats()
        {
            // Arrange
            //Setup();

            // Act
            

            // Assert
            Assert.IsNotNull(taskService.GetCats(null, null, null, null));            
            Assert.IsNotNull(taskService.GetCats("color", null, null, null));            
            Assert.IsNotNull(taskService.GetCats("color", "desc", null, null));            
            Assert.IsNotNull(taskService.GetCats(null, null, 5, null));            
            Assert.IsNotNull(taskService.GetCats(null, null, null, 3));            
            Assert.IsNotNull(taskService.GetCats(null, null, 5, 3));            
            Assert.IsNotNull(taskService.GetCats("color", "desc", 3, 8));            
        }
        [Test]
        public void TestValidationException()
        {
            // Arrange
            //ITaskService taskService = new TaskServices();
            //Setup();
            // Act
            

            // Assert
            Assert.Throws<ValidationException>(()=>taskService.GetCats("names", null, null, null));
            Assert.Throws<ValidationException>(()=>taskService.GetCats("name", "descend", null, null));
            Assert.Throws<ValidationException>(() => taskService.GetCats("name", "desc", -1, null));
            Assert.Throws<ValidationException>(() => taskService.GetCats("name", "desc", 1, 0));

        }
    }
}