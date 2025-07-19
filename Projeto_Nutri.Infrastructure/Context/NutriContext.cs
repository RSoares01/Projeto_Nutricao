using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;

namespace Projeto_Nutri.Infrastructure.Context
{
    public class NutriContext : DbContext
    {
        public NutriContext(DbContextOptions<NutriContext> options) : base(options) { }

        public DbSet<Foods> Foods { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<MealPlans> MealPlans { get; set; }
        public DbSet<MealPlanFoods> MealPlanFoods { get; set; } // ⬅️ AQUI!

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Foods>().ToTable("Foods");
            modelBuilder.Entity<Patients>().ToTable("Patients");
            modelBuilder.Entity<MealPlans>().ToTable("MealPlans");
            modelBuilder.Entity<MealPlanFoods>().ToTable("MealPlanFoods");

            modelBuilder.Entity<Foods>().HasKey(f => f.Id);
            modelBuilder.Entity<Patients>().HasKey(p => p.Id);
            modelBuilder.Entity<MealPlans>().HasKey(m => m.Id);
            modelBuilder.Entity<MealPlanFoods>().HasKey(mf => mf.Id);

            // Filtro global para ignorar pacientes marcados como deletados
            modelBuilder.Entity<Patients>().HasQueryFilter(p => !p.IsDeleted);


            modelBuilder.Entity<MealPlans>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MealPlans)
                .HasForeignKey(m => m.PatientId);

            modelBuilder.Entity<MealPlanFoods>()
                .HasOne(mf => mf.MealPlan)
                .WithMany(mp => mp.Alimentos)
                .HasForeignKey(mf => mf.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade); // ⬅️ Isso ajuda na exclusão em cascata

            modelBuilder.Entity<MealPlanFoods>()
                .HasOne(mf => mf.Food)
                .WithMany(f => f.UsosEmPlanos)
                .HasForeignKey(mf => mf.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
