using MesModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesContexts.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Etablissement> Etablissements { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<PhotosEtablissement> PhotosEtablissements { get; set; }
        public DbSet<Horaires> Horaires { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Etablissement>()
                .HasMany<Horaires>(e => e._HoraireList)
                .WithOne(x => x.Etablissement)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Etablissement>()
                .HasMany<PhotosEtablissement>(e => e._PhotosList)
                .WithOne(x => x.Etablissement)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
