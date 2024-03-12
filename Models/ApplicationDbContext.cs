using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RESTapiPruebaTeen.Models.Clases;

namespace RESTapiPruebaTeen.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Chiste> Chistes { get; set; } // DbSet para la entidad Chiste

        public ApplicationDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chiste>().HasKey(c => c.ID);
        }
    }
}