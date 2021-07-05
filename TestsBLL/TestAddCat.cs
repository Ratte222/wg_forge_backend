using NUnit.Framework;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using BLL.Infrastructure;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace TestsBLL
{
    public class TestAddCat
    {
        private ITaskService taskService;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder<CatContext> options = new DbContextOptionsBuilder<CatContext>();
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wg_forge_backend;Trusted_Connection=True;MultipleActiveResultSets=true");
            CatContext catContext = new CatContext(options.Options);
            //taskService = new TaskServices(catContext);
        }

        //[Test]
        //public void TestAddCatInvalidData()
        //{
        //    //invalid json
        //    Assert.Throws<JsonException>(() => taskService.AddCat(
        //        "{\"name\": \"Tihon\", \"color\": \"red & white\", \"tail_length\": 15, \"whiskers_length\": 12"));
        //    //name exist
        //    ValidationException ex = Assert.Throws<ValidationException>(() => taskService.AddCat(
        //        "{\"name\": \"Tihon\", \"color\": \"red & white\", \"tail_length\": 15, \"whiskers_length\": 12}"));
        //    Assert.AreEqual("A cat with the same name already exists", ex.Message);
        //    //invalid color
        //    System.ComponentModel.DataAnnotations.ValidationException ex2 = Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(() => taskService.AddCat(
        //        "{\"name\": \"Artur\", \"color\": \"blue & white\", \"tail_length\": 10, \"whiskers_length\": 12}"));
        //    Assert.AreEqual("There is no such color of a cat", ex2.Message);
        //    //Tsil length out of range
        //    ex2 = Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(() => taskService.AddCat(
        //        "{\"name\": \"Artur\", \"color\": \"red & white\", \"tail_length\": -1, \"whiskers_length\": 12}"));
        //    Assert.AreEqual("Value for TailLength must be between 0 and 43.", ex2.Message);
        //    //whiskers length out of range
        //    ex2 = Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(() => taskService.AddCat(
        //        "{\"name\": \"Artur\", \"color\": \"red & white\", \"tail_length\": 4, \"whiskers_length\": -1}"));
        //    Assert.AreEqual("Value for WhiskersLength must be between 0 and 20.", ex2.Message);
        //    //Wintout parameter name
        //    ex2 = Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(() => taskService.AddCat(
        //        "{\"color\": \"red & white\", \"tail_length\": 4, \"whiskers_length\": 5}"));
        //    Assert.AreEqual("The Name field is required.", ex2.Message);
        //}

        //[Test]
        //public void TestCatValidData()
        //{
        //    Assert.DoesNotThrow(() => taskService.AddCat("{\"name\": \"Artur\", \"color\": \"red & white\", \"tail_length\": 15, \"whiskers_length\": 12}"));
        //}
    }
}
