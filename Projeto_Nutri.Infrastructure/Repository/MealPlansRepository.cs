using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<MealPlans?> GetByIdAsync(int id)
        {
            return await Context.MealPlans
                .Include(m => m.Patient)
                .Include(m => m.Alimentos)
                    .ThenInclude(a => a.Food)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MealPlans>> GetAllAsync()
        {
            return await Context.MealPlans
                .Include(m => m.Patient)
                .Include(m => m.Alimentos)
                    .ThenInclude(a => a.Food)
                .ToListAsync();
        }

        public async Task CreateAsync(MealPlans mealPlans)
        {
            await Context.MealPlans.AddAsync(mealPlans);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mealPlan = await Context.MealPlans
                .Include(m => m.Alimentos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mealPlan != null)
            {
                if (mealPlan.Alimentos != null && mealPlan.Alimentos.Any())
                {
                    Context.MealPlanFoods.RemoveRange(mealPlan.Alimentos);
                }

                Context.MealPlans.Remove(mealPlan);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MealPlans>> GetTodayByPatientIdAsync(int patientId)
        {
            var today = DateTime.Today;

            return await _context.MealPlans
                .Where(mp => mp.PatientId == patientId && mp.DataCriacao.Date == today)
                .Include(m => m.Patient)
                .Include(m => m.Alimentos)
                    .ThenInclude(a => a.Food)
                .ToListAsync();
        }
    }
}
