using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Nutri.Application.Service
{
    public class PatientsService
    {
        private readonly IPatientsRepository _patientsRepository;

        public PatientsService(IPatientsRepository patientsRepository)
        {
            _patientsRepository = patientsRepository;
        }

        public async Task<PatientsDTO?> GetPatientByIdAsync(int id)
        {
            var patient = await _patientsRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return null;
            }

            return new PatientsDTO
            {
                Id = patient.Id,
                Nome = patient.Nome,
                Idade = patient.Idade,
                Genero = patient.Genero,
            };
        }

        public async Task<IEnumerable<PatientsDTO>> GetAllPatientsAsync()
        {
            var patients = await _patientsRepository.GetAllAsync();

            return patients.Select(p => new PatientsDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade,
                Genero = p.Genero
            });
        }

        public async Task<Patients> CreatePatientAsync(PatientsDTO patientsDTO)
        {
            var patient = new Patients(patientsDTO.Nome, patientsDTO.Idade, patientsDTO.Genero, patientsDTO.DataCriacao);
            await _patientsRepository.CreateAsync(patient);
            return patient;
        }

        public async Task<Patients> UpdatePatientAsync(PatientsDTO patientsDTO)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientsDTO.Id);

            if (patient == null)
                throw new Exception("Paciente não encontrado.");

            patient.Nome = patientsDTO.Nome;
            patient.Genero = patientsDTO.Genero;
            patient.Idade = patientsDTO.Idade;

            await _patientsRepository.UpdateAsync(patient);

            return patient;
        }

        public async Task RemovePatientAsync(int id)
        {
            await _patientsRepository.DeleteAsync(id);
        }
    }
}
