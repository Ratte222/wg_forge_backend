using NUnit.Framework;
using BLL.Interfaces;
using BLL.Services;
using BLL.DTO;
using BLL.Infrastructure;
using System.Collections.Generic;

namespace TestsBLL
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetCats()
        {
            // Arrange
            ITaskService taskService = new TaskServices();

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
            ITaskService taskService = new TaskServices();

            // Act
            

            // Assert
            Assert.Throws<ValidationException>(()=>taskService.GetCats("names", null, null, null));
            Assert.Throws<ValidationException>(()=>taskService.GetCats("name", "descend", null, null));
            Assert.Throws<ValidationException>(() => taskService.GetCats("name", "desc", -1, null));
            Assert.Throws<ValidationException>(() => taskService.GetCats("name", "desc", 1, 0));

        }
    }
}