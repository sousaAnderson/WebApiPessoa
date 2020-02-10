using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPessoas.Models;

namespace WebApiPessoas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :
           base(options)
        { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .HasKey(p => p.Codigo);
            modelBuilder.Entity<Usuario>()
                .HasKey(p => p.ID);
        }
    }
}
