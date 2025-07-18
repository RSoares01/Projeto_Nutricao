using Projeto_Nutri.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Infrastructure.IRepository
{
    public interface IPatientsRepository
    {
        Patients GetById(int id);
        IEnumerable<Patients> GetAll();
        void Create(Patients patients);
        void Update(Patients patients);
        void Delete(int id);
    }
}
