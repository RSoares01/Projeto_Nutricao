using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Domain.Entity
{
    public class MealPlanFoods
    {
        public int Id { get; set; }

        public int MealPlanId { get; set; }
        public MealPlans MealPlan { get; set; } = null!;

        public int FoodId { get; set; }
        public Foods Food { get; set; } = null!;

        public decimal TamanhoDaPorcaoEmGramas { get; set; }
    }
}
