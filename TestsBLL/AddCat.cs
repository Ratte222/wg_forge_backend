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
    public class AddCat
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
        public void TestAddCatInvalidData()
        {
            //invalid json
            Assert.Throws<JsonException>(() => taskService.AddCat(
                "{\"name\": \"Tihon\", \"color\": \"red & white\", \"tail_length\": 15, \"whiskers_length\": 12"));
            //name exist
            Assert.Throws<ValidationException>(() => taskService.AddCat(
                "{\"name\": \"Tihon\", \"color\": \"red & white\", \"tail_length\": 15, \"whiskers_length\": 12}"));
            //Tsil length out of range
            Assert.Throws<ValidationException>(() => taskService.AddCat(
                "{\"name\": \"Artur\", \"color\": \"red & white\", \"tail_length\": -1, \"whiskers_length\": 12}"));
        }
    }
}
