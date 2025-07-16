using Projeto_Nutri.Domain.Entity;

namespace Projeto_Nutri.Infrastructure.IRepository
{
    public interface IFoodsRepository
    {
        Foods ObterPorId(int id);
        IEnumerable<Foods> ObterTodos();
        void Adicionar(Foods food);
        void Atualizar(Foods food);
        void Remover(int id);
    }
}
