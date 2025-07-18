using Projeto_Nutri.Domain.Entity;

namespace Projeto_Nutri.Infrastructure.IRepository
{
    public interface IFoodsRepository
    {
        Foods GetById(int id);
        IEnumerable<Foods> GetAll();
        void Create(Foods food);
        void Update(Foods food);
        void Delete(int id);
    }
}
