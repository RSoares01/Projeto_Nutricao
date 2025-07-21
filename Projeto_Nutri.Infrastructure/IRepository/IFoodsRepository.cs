using Projeto_Nutri.Domain.Entity;

public interface IFoodsRepository
{
    Task<Foods?> GetByIdAsync(int id);
    Task<IEnumerable<Foods>> GetAllAsync();
    Task CreateAsync(Foods food);
    Task UpdateAsync(Foods food);
    Task DeleteAsync(int id);
}
