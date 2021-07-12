using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace DAL.EF
{
    public class CatContext: DbContext
    {
        public DbSet<Cat> Cats { get; set; }
        public DbSet<CatColorInfo> CatColorInfos { get; set; }
        public DbSet<CatStat> CatStats { get; set; }
        public DbSet<CatOwner> CatOwners { get; set; }

        public CatContext(DbContextOptions<CatContext> options):base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            StoreDbInitializer.Initialize(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=wg_forge_backend;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatColorInfo>().HasKey(u => u.Color)/*.HasAlternateKey(u => u.Color)*/;
            //modelBuilder.Entity<CatStat>().HasNoKey();
        }

        

    }

    public static class StoreDbInitializer
    {
        public static void Initialize(CatContext db)
        {
            if(!db.CatOwners.Any()&& !db.Cats.Any())
            {
                CatOwner Artur = new CatOwner()
                {
                    Name = "Artur",
                    Age = 21
                    //Cats = new List<Cat>() { new Cat() { Name = "Tihon" } }
                };
                CatOwner Niki = new CatOwner() { Name = "Niki", Age = 22 };
                CatOwner Valera = (new CatOwner() { Name = "Valera", Age = 20 });
                CatOwner Vika = (new CatOwner() { Name = "Vika", Age = 26 });
                CatOwner Den = (new CatOwner() { Name = "Den", Age = 27 });
                CatOwner Sasha = (new CatOwner() { Name = "Sasha", Age = 35 });
                CatOwner Diana = (new CatOwner() { Name = "Diana", Age = 17 });
                db.CatOwners.AddRange(Artur, Niki, Valera, Vika, Den, Sasha, Diana);
                Cat Tihon = new Cat { Name = "Tihon", Color = "red & white", TailLength = 15, WhiskersLength = 12 };
                Cat Marfa = new Cat { Name = "Marfa", Color = "black & white", TailLength = 13, WhiskersLength = 11 };
                Cat Asya = new Cat { Name = "Asya", Color = "black", TailLength = 10, WhiskersLength = 10 };
                Cat Amur = (new Cat { Name = "Amur", Color = "black & white", TailLength = 20, WhiskersLength = 11 });
                Cat Hustav = (new Cat { Name = "Hustav", Color = "red & white", TailLength = 12, WhiskersLength = 12 });
                Cat Dina = (new Cat { Name = "Dina", Color = "black & white", TailLength = 17, WhiskersLength = 12 });
                Cat Gass =(new Cat { Name = "Gass", Color = "red & white", TailLength = 15, WhiskersLength = 13 });
                Cat CVika = (new Cat { Name = "Vika", Color = "black", TailLength = 14, WhiskersLength = 10 });
                Cat Cold = (new Cat { Name = "Clod", Color = "red & white", TailLength = 12, WhiskersLength = 15 });
                Cat Neo = (new Cat { Name = "Neo", Color = "red", TailLength = 11, WhiskersLength = 13 });
                Cat Nord = (new Cat { Name = "Nord", Color = "red & black & white", TailLength = 19, WhiskersLength = 12 });
                Cat Kelly = (new Cat { Name = "Kelly", Color = "red & white", TailLength = 26, WhiskersLength = 11 });
                Cat Ost = (new Cat { Name = "Ost", Color = "white", TailLength = 14, WhiskersLength = 12 });
                Cat Tayson = (new Cat { Name = "Tayson", Color = "red & white", TailLength = 18, WhiskersLength = 13 });
                Cat Lesya = (new Cat { Name = "Lesya", Color = "black & white", TailLength = 12, WhiskersLength = 15 });
                Cat Foma = (new Cat { Name = "Foma", Color = "black", TailLength = 15, WhiskersLength = 18 });
                Cat Oddet = (new Cat { Name = "Odett", Color = "red & white", TailLength = 17, WhiskersLength = 13 });
                Cat Cesar = (new Cat { Name = "Cesar", Color = "black & white", TailLength = 18, WhiskersLength = 14 });
                Cat Shurik = (new Cat { Name = "Shurik", Color = "red & white", TailLength = 17, WhiskersLength = 13 });
                Cat Flora = (new Cat { Name = "Flora", Color = "black & white", TailLength = 12, WhiskersLength = 15 });
                Cat Tara = (new Cat { Name = "Tara", Color = "red & white", TailLength = 17, WhiskersLength = 12 });
                Cat Yasha = (new Cat { Name = "Yasha", Color = "red & white", TailLength = 18, WhiskersLength = 12 });
                Cat Chlo = (new Cat { Name = "Chlo", Color = "black", TailLength = 14, WhiskersLength = 13 });
                Cat Snow = (new Cat { Name = "Snow", Color = "white", TailLength = 19, WhiskersLength = 14 });
                Cat Sam = (new Cat { Name = "Sam", Color = "black & white", TailLength = 15, WhiskersLength = 15 });
                Cat Ula = (new Cat { Name = "Ula", Color = "red & white", TailLength = 16, WhiskersLength = 14 });
                db.Cats.AddRange(Tihon, Marfa, Asya, Amur, Hustav, Dina, Gass, CVika, Cold, Neo, Nord,
                    Kelly, Ost, Tayson, Lesya, Foma, Oddet, Cesar, Shurik, Flora, Tara, Yasha, Chlo, Snow, Sam, Ula);
                Artur.Cats.AddRange(new List<Cat> { Tihon, Marfa });
                Niki.Cats.AddRange(new List<Cat> {  Asya, Amur, Ula });
                Valera.Cats.AddRange(new List<Cat> { Hustav, Dina, Gass, CVika, Cold, Neo });
                Vika.Cats.AddRange(new List<Cat> { Nord, Kelly, Ost, Tayson });
                Den.Cats.AddRange(new List<Cat> { Lesya, Foma, Oddet, Cesar, Shurik, Flora, Tara, Yasha, Chlo, Snow });
                Sasha.Cats.AddRange(new List<Cat> { Sam });
                Diana.Cats.AddRange(new List<Cat> { Asya, Amur, Ula });
                db.SaveChanges();
            }
        }
    }
}
