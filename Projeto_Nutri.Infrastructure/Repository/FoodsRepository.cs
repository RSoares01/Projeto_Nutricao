using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Nutri.Infrastructure.Repository
{
    public class FoodsRepository : IFoodsRepository
    {
        private readonly NutriContext _context;

        public NutriContext Context => _context;

        public FoodsRepository(NutriContext context)
        {
            _context = context;
        }

        public async Task<Foods?> GetByIdAsync(int id)
        {
            return await Context.Foods
                .Include(f => f.UsosEmPlanos)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Foods>> GetAllAsync()
        {
            return await Context.Foods.ToListAsync();
        }

        public async Task CreateAsync(Foods food)
        {
            await Context.Foods.AddAsync(food);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Foods food)
        {
            Context.Foods.Update(food);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var food = await Context.Foods
                .Include(f => f.UsosEmPlanos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food != null)
            {
                if (food.UsosEmPlanos.Any())
                    Context.MealPlanFoods.RemoveRange(food.UsosEmPlanos);

                Context.Foods.Remove(food);
                await Context.SaveChangesAsync();
            }
        }
    }
}
