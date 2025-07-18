using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Linq;

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

        public Foods GetById(int id)
        {
            return Context.Foods.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Foods> GetAll()
        {
            return Context.Foods.ToList();
        }

        public void Create(Foods food)
        {
            Context.Foods.Add(food);
            Context.SaveChanges();
        }

        public void Update(Foods food)
        {
            Context.Foods.Update(food);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var food = Context.Foods.FirstOrDefault(f => f.Id == id);
            if (food != null)
            {
                Context.Foods.Remove(food);
                Context.SaveChanges();
            }
        }
    }
}
