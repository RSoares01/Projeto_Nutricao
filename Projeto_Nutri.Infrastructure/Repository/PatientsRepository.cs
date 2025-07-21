using Microsoft.EntityFrameworkCore;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Patients?> GetByIdAsync(int id)
        {
            return await Context.Patients.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Patients>> GetAllAsync()
        {
            return await Context.Patients.ToListAsync();
        }

        public async Task CreateAsync(Patients patients)
        {
            await Context.Patients.AddAsync(patients);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patients patients)
        {
            Context.Patients.Update(patients);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await Context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient != null)
            {
                patient.IsDeleted = true;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MealPlans>> GetTodayByPatientIdAsync(int patientId)
        {
            var today = DateTime.Today;
            return await _context.MealPlans
                .Where(mp => mp.PatientId == patientId && mp.DataCriacao.Date == today)
                .ToListAsync();
        }
    }
}
