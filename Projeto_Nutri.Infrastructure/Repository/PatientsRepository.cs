using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Nutri.Infrastructure.Repository
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly NutriContext _context;

        public NutriContext Context => _context;

        public PatientsRepository(NutriContext context)
        {
            _context = context;
        }

        public Patients GetById(int id)
        {
            return Context.Patients.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Patients> GetAll()
        {
            return Context.Patients.ToList();
        }

        public void Create(Patients patients)
        {
            Context.Patients.Add(patients);
            Context.SaveChanges();
        }

        public void Update(Patients patients)
        {
            Context.Patients.Update(patients);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var food = Context.Patients.FirstOrDefault(p => p.Id == id);
            if (food != null)
            {
                Context.Patients.Remove(food);
                Context.SaveChanges();
            }
        }
    }
}
