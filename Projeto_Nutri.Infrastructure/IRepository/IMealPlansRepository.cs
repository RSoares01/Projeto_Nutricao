using Projeto_Nutri.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Infrastructure.IRepository
{
    public interface IMealPlansRepository
    {
        MealPlans GetById(int id);
        IEnumerable<MealPlans> GetAll();
        void Create(MealPlans mealPlans);
        void Update(MealPlans mealPlans);
        void Delete(int id);
    }
}
