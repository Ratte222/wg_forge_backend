using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using AutoMapper;
using DAL.Interface;
using DAL.Entities;
using BLL.Services;
using BLL.Infrastructure;
using BLL.DTO;
using wg_forge_backend;

namespace Tests
{
    public class TaskServicesTests
    {
        Mock<IRepository<Cat>> mockRepoCat;
        Mock<IRepository<CatColorInfo>> mockRepoCatColorInfo;
        Mock<IRepository<CatStat>> mockRepoCatStat;
        MapperConfiguration mapperConfig;
        IMapper _mapper;
        TaskServices taskServices;
        NewCatDTO newCatDTO_Artur = new NewCatDTO()
        {
            Name = "Artur",
            Color = "black",
            TailLength = 10,
            WhiskersLength = 16
        };
        NewCatDTO newCatDTO_Tihon = new NewCatDTO()
        {
            Name = "Tihon",
            Color = "black",
            TailLength = 10,
            WhiskersLength = 16
        };
        private void Setup()
        {
            mockRepoCat = new Mock<IRepository<Cat>>();
            mockRepoCatColorInfo =
            new Mock<IRepository<CatColorInfo>>();
            mockRepoCatStat =
            new Mock<IRepository<CatStat>>();
            mapperConfig =
            new MapperConfiguration(cfg => cfg.AddProfile(typeof(CatProfile)));
            _mapper = mapperConfig.CreateMapper();
            taskServices = new TaskServices(mockRepoCat.Object,
                mockRepoCatColorInfo.Object,
                mockRepoCatStat.Object, _mapper);
        }

        [Fact]
        public void AddCatReturnEmpty()
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());
            //Act
            taskServices.AddCat(newCatDTO_Artur);
            //Assert
            mockRepoCat.Verify(i=>i.Create(It.IsAny<Cat>()));            
        }

        [Fact]
        public void AddCatReturnValidationExeption()
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());
            //Act
            //Assert
            Assert.Throws<ValidationException>(() => taskServices.AddCat(newCatDTO_Tihon));
        }

        [Fact]
        public void EditCatReturnEmpty()
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());
            //Act
            taskServices.EditCat(newCatDTO_Tihon);
            //taskServices.EditCat(_mapper.Map<NewCatDTO, CatDTO>(newCatDTO_Tihon));
            //Assert
            mockRepoCat.Verify(i => i.Update(It.IsAny<Cat>()));
        }

        [Fact]
        public void EditCatReturnValidationExeption()
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());            
            //Act
            //Assert
            Assert.Throws<ValidationException>(() => 
                taskServices.EditCat(newCatDTO_Artur));            
                //taskServices.EditCat(_mapper.Map<NewCatDTO, CatDTO>(newCatDTO_Artur)));            
        }

        [Fact]
        public void DeleteCatReturnEmpty()
        {
            //Arrange
            Setup();
            //Act
            taskServices.DeleteCat(_mapper.Map<NewCatDTO, CatDTO>(newCatDTO_Artur));
            //Assert
            mockRepoCat.Verify(i => i.Delete(It.IsAny<Cat>()));
        }

        [Theory]
        [InlineData(null, null, null, null, 4)]
        [InlineData("Color", null, null, null, 4)]
        [InlineData("Color", "desc", null, null, 4)]
        [InlineData(null, null, 2, null, 2)]
        [InlineData(null, null, null, 3, 3)]
        [InlineData(null, null, 2, 3, 2)]
        [InlineData("Color", "desc", 3, 8, 1)]
        public void GetCatsReturnEnumerable(string attribute, string order,
            int? offset, int? limit, int count)
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());
            //Act
            var result = taskServices.GetCats(attribute, order, offset, limit);
            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(count, result.Count);
        }

        [Theory]
        [InlineData("Names", null, null, null, @"The ""attribute"" parameter is not correct. " +
                    @"Use ""Name"" or ""Color"" or ""TailLength"" or ""WhiskersLength""")]
        [InlineData("Name", "descend", null, null, @"The ""order"" parameter "+
                    @"is not correct. Use ""asc"" or ""desc""")]
        [InlineData("Name", "desc", 10, null, @"The ""offset"" >= cats count")]
        [InlineData("Name", "desc", -1, null, @"The ""offset"" cannot be less 0")]  
        [InlineData("Name", "desc", 1, 0, @"The ""limit"" cannot be less 1")]
        public void GetCatsReturnValidationException(string attribute, string order,
            int? offset, int? limit, string message)
        {
            //Arrange
            Setup();
            mockRepoCat.Setup(task => task.GetAll_Queryable()).
                Returns(GetTestCats().AsQueryable<Cat>());
            //Act

            //Assert
            ValidationException ex = Assert.Throws<ValidationException>(
                ()=> taskServices.GetCats(attribute, order, offset, limit));
            Assert.Equal(message, ex.Message);
        }

        private List<Cat> GetTestCats()
        {
            var cats = new List<Cat>
            {
                new Cat { Name = "Tihon", Color = "red & white", TailLength = 15, WhiskersLength = 12 },
                new Cat { Name = "Marfa", Color = "black & white", TailLength = 13, WhiskersLength = 11 },
                new Cat { Name = "Kelly", Color = "red & white", TailLength = 26, WhiskersLength = 11 },
                new Cat { Name = "Flora", Color = "black & white", TailLength = 12, WhiskersLength = 15 }
            };
            return cats;
        }

        [Fact]
        public void PingReturnString()
        {
            //Arrange
            Setup();
            //Act
            var result = taskServices.Ping();
            //Assert
            Assert.Equal("Cats Service. Version 0.1", result);
        }
    }
}
