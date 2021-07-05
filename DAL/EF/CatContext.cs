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

        public CatContext(DbContextOptions<CatContext> options):base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
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
            if(!db.Cats.Any())
            {
                db.Cats.Add(new Cat { Name = "Tihon", Color = "red & white", TailLength = 15, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Marfa", Color = "black & white", TailLength = 13, WhiskersLength = 11 });
                db.Cats.Add(new Cat { Name = "Asya", Color = "black", TailLength = 10, WhiskersLength = 10 });
                db.Cats.Add(new Cat { Name = "Amur", Color = "black & white", TailLength = 20, WhiskersLength = 11 });
                db.Cats.Add(new Cat { Name = "Hustav", Color = "red & white", TailLength = 12, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Dina", Color = "black & white", TailLength = 17, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Gass", Color = "red & white", TailLength = 15, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Vika", Color = "black", TailLength = 14, WhiskersLength = 10 });
                db.Cats.Add(new Cat { Name = "Clod", Color = "red & white", TailLength = 12, WhiskersLength = 15 });
                db.Cats.Add(new Cat { Name = "Neo", Color = "red", TailLength = 11, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Nord", Color = "red & black & white", TailLength = 19, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Kelly", Color = "red & white", TailLength = 26, WhiskersLength = 11 });
                db.Cats.Add(new Cat { Name = "Ost", Color = "white", TailLength = 14, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Tayson", Color = "red & white", TailLength = 18, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Lesya", Color = "black & white", TailLength = 12, WhiskersLength = 15 });
                db.Cats.Add(new Cat { Name = "Foma", Color = "black", TailLength = 15, WhiskersLength = 18 });
                db.Cats.Add(new Cat { Name = "Odett", Color = "red & white", TailLength = 17, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Cesar", Color = "black & white", TailLength = 18, WhiskersLength = 14 });
                db.Cats.Add(new Cat { Name = "Shurik", Color = "red & white", TailLength = 17, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Flora", Color = "black & white", TailLength = 12, WhiskersLength = 15 });
                db.Cats.Add(new Cat { Name = "Tara", Color = "red & white", TailLength = 17, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Yasha", Color = "red & white", TailLength = 18, WhiskersLength = 12 });
                db.Cats.Add(new Cat { Name = "Chlo", Color = "black", TailLength = 14, WhiskersLength = 13 });
                db.Cats.Add(new Cat { Name = "Snow", Color = "white", TailLength = 19, WhiskersLength = 14 });
                db.Cats.Add(new Cat { Name = "Sam", Color = "black & white", TailLength = 15, WhiskersLength = 15 });
                db.Cats.Add(new Cat { Name = "Ula", Color = "red & white", TailLength = 16, WhiskersLength = 14 });
                db.Cats.Add(new Cat { Name = "Nemo", Color = "red & white", TailLength = 17, WhiskersLength = 13 });
                db.SaveChanges();
            }
        }
    }
}
