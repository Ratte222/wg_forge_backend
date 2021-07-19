using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DAL.InitData;

namespace DAL.EF
{
    public class CatContext: IdentityDbContext
    {
        public DbSet<Cat> Cats { get; set; }
        public DbSet<CatColorInfo> CatColorInfos { get; set; }
        public DbSet<CatStat> CatStats { get; set; }
        public DbSet<CatOwner> CatOwners { get; set; }
        public DbSet<CatPhoto> CatPhotos { get; set; }

        public CatContext() { }

        public CatContext(DbContextOptions<CatContext> options):base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //StoreDbInitializer.Initialize(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=wg_forge_backend;Trusted_Connection=True;");               
                
                //var builder = new ConfigurationBuilder();
                //// установка пути к текущему каталогу
                //builder.SetBasePath(Directory.GetCurrentDirectory());
                //// получаем конфигурацию из файла appsettings.json
                //builder.AddJsonFile("appsettings.json");
                //// создаем конфигурацию
                //var config = builder.Build();
                //// получаем строку подключения
                //optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CatsAndOwners>()
                .HasKey(k => new { k.CatOwnersId, k.CatsId });
            modelBuilder.Entity<Cat>().HasKey(i => i.Id);

            modelBuilder.Entity<CatsAndOwners>()
                .HasOne(cao => cao.CatOwner)
                .WithMany(co => co.CatsAndOwners)
                .HasForeignKey(cao => cao.CatOwnersId);
            modelBuilder.Entity<CatsAndOwners>()
                .HasOne(cao => cao.Cat)
                .WithMany(co => co.CatsAndOwners)
                .HasForeignKey(cao => cao.CatsId);

            modelBuilder.Entity<CatPhoto>().HasKey(i => i.Id);
            modelBuilder.Entity<CatPhoto>()
                .HasOne(cp => cp.Cat)
                .WithMany(c => c.CatPhotos)
                .HasForeignKey(cp => cp.CatId);
            modelBuilder.Entity<CatColorInfo>().HasKey(u => u.Color)/*.HasAlternateKey(u => u.Color)*/;
            //modelBuilder.Entity<CatStat>().HasNoKey();
        }
    }   
}
