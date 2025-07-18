using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Projeto_Nutri.Infrastructure.Context
{
    public class NutriContext : DbContext
    {
        public NutriContext(DbContextOptions<NutriContext> options)
            : base(options)
        {
        }

        public DbSet<Foods> Foods { get; set; }
        public DbSet<Patients> Patients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //tabelas 
            modelBuilder.Entity<Foods>().ToTable("Foods");
            modelBuilder.Entity<Patients>().ToTable("Patients");

            //Chaves primárias
            modelBuilder.Entity<Foods>().HasKey(f => f.Id);
            modelBuilder.Entity<Patients>().HasKey(p => p.Id);


        }
    }
}