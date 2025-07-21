using Projeto_Nutri.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projeto_Nutri.Infrastructure.IRepository
{
    public interface IPatientsRepository
    {
        Task<Patients?> GetByIdAsync(int id);
        Task<IEnumerable<Patients>> GetAllAsync();
        Task CreateAsync(Patients patient);
        Task UpdateAsync(Patients patient);
        Task DeleteAsync(int id);
    }
}
