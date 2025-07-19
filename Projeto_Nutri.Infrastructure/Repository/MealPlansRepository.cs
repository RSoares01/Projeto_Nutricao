using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;

namespace Projeto_Nutri.Infrastructure.Repository
{
    public class MealPlansRepository : IMealPlansRepository
    {
        private readonly NutriContext _context;

        public NutriContext Context => _context;

        public MealPlansRepository(NutriContext context)
        {
            _context = context;
        }

        public MealPlans? GetById(int id)
        {
            return Context.MealPlans
                .Include(m => m.Patient)
                .Include(m => m.Alimentos)
                    .ThenInclude(a => a.Food)
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<MealPlans> GetAll()
        {
            return Context.MealPlans
                .Include(m => m.Patient)
                .Include(m => m.Alimentos)
                    .ThenInclude(a => a.Food)
                .ToList();
        }

        public void Create(MealPlans mealPlans)
        {
            Context.MealPlans.Add(mealPlans);
            Context.SaveChanges();
        }

        public void Update(MealPlans mealPlans)
        {
            Context.MealPlans.Update(mealPlans);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mealPlan = Context.MealPlans
                .Include(m => m.Alimentos)
                .FirstOrDefault(m => m.Id == id);

            if (mealPlan != null)
            {
                // Remove os alimentos filhos antes, se necessário
                if (mealPlan.Alimentos != null && mealPlan.Alimentos.Any())
                {
                    Context.MealPlanFoods.RemoveRange(mealPlan.Alimentos);
                }

                Context.MealPlans.Remove(mealPlan);
                Context.SaveChanges();
            }
        }
    }
}
