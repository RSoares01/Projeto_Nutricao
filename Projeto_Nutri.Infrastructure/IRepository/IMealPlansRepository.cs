using Projeto_Nutri.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMealPlansRepository
{
    Task<IEnumerable<MealPlans>> GetAllAsync();
    Task<MealPlans?> GetByIdAsync(int id);
    Task CreateAsync(MealPlans mealPlan);
    Task DeleteAsync(int id);
    Task<IEnumerable<MealPlans>> GetTodayByPatientIdAsync(int patientId);
}
