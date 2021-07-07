using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using BLL.Interfaces;
using wg_forge_backend.Controllers;
using BLL.DTO;
using System.Collections.Generic;

namespace Tests
{
    public class CatsControllerTests
    {
        Mock<ITaskService> mockITaskService = new Mock<ITaskService>();
        NewCatDTO newCatDTO = new NewCatDTO()
        {
            Name = "Artur",
            Color = "black",
            TailLength = 10,
            WhiskersLength = 16
        };
        CatDTO catDTO = new CatDTO()
        {
            Name = "Artur",
            Color = "black",
            TailLength = 10,
            WhiskersLength = 16
        };
        [Fact]
        public void PingTest()
        {
            //Arrange

            mockITaskService.Setup(task => task.Ping()).Returns("Cats Service. Version 0.1");
            var controller = new CatsController(mockITaskService.Object);
            //Act
            var result = controller.Ping();
            //Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Cats Service. Version 0.1", contentResult.Content);
        }

        [Fact]
        public void GetCatsReturnJson()
        {
            //Arrange
            mockITaskService.Setup(task => task.GetCats(null, null, null, null)).
                Returns(GetTestCats());
            var controller = new CatsController(mockITaskService.Object);
            //Act
            var result = controller.Cats(null, null, null, null);
            //Assert
            Assert.IsType<JsonResult>(result);
        }

        private List<CatDTO> GetTestCats()
        {
            var cats = new List<CatDTO>
            {
                new CatDTO { Name = "Tihon", Color = "red & white", TailLength = 15, WhiskersLength = 12 },
                new CatDTO { Name = "Marfa", Color = "black & white", TailLength = 13, WhiskersLength = 11 },
                new CatDTO { Name = "Kelly", Color = "red & white", TailLength = 26, WhiskersLength = 11 },
                new CatDTO { Name = "Flora", Color = "black & white", TailLength = 12, WhiskersLength = 15 }
            };
            return cats;
        }

        [Fact]
        public void AddCatReturnStatusCode200()
        {
            //Arrange
            var controller = new CatsController(mockITaskService.Object);

            //Act
            var result = controller.AddNewCat(newCatDTO);
            //Assert
            var statusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusCode.StatusCode);
            mockITaskService.Verify(r => r.AddCat(newCatDTO));
        }

        [Fact]
        public void AddCatReturnBadRequest400()
        {
            //Arrange
            var controller = new CatsController(mockITaskService.Object);
            controller.ModelState.AddModelError("Name", "Required");
            controller.ModelState.AddModelError("Color", "Required");
            controller.ModelState.AddModelError("TailLength", "Required");
            controller.ModelState.AddModelError("WhiskersLength", "Required");
            newCatDTO.Name = null;
            //Act
            var result = controller.AddNewCat(newCatDTO);
            //Assert
            var badRequest = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public void EditCatReturnStatusCode200()
        {
            //Arrange
            var controller = new CatsController(mockITaskService.Object);
            
            //Act
            var result = controller.CatEdit(newCatDTO);
            //Assert
            var statusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusCode.StatusCode);
            mockITaskService.Verify(r => r.EditCat(newCatDTO));
        }

        [Fact]
        public void DeleteCatReturnStatusCode200()
        {
            //Arrange
            var controller = new CatsController(mockITaskService.Object);
            
            //Act
            var result = controller.CatDelete(catDTO);
            //Assert
            var statusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusCode.StatusCode);
            mockITaskService.Verify(r => r.DeleteCat(catDTO));
        }

        [Fact]
        public void DeleteCatReturnBadRequest400()
        {
            //Arrange
            var controller = new CatsController(mockITaskService.Object);
            controller.ModelState.AddModelError("Name", "Required");
            catDTO.Name = null;
            //Act
            var result = controller.CatDelete(catDTO);
            //Assert
            var badRequest = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequest.StatusCode);            
        }
    }
}
