using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vjezba.Model;

namespace Vjezba.DAL
{
    public class ReservationsDbContext : DbContext
    {
        public ReservationsDbContext(DbContextOptions<ReservationsDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Stol> Stolovi { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                b.Property(t => t.LoginProvider).HasMaxLength(128);
                b.Property(t => t.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Stol>().HasData(new Stol { Id = 1, Broj_Stolica = 2 });
            modelBuilder.Entity<Stol>().HasData(new Stol { Id = 2, Broj_Stolica = 3 });
            modelBuilder.Entity<Stol>().HasData(new Stol { Id = 3, Broj_Stolica = 4 });



        }
    }
}
