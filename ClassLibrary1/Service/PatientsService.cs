using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;

namespace Projeto_Nutri.Application.Service
{
    public class PatientsService
    {
        private readonly IPatientsRepository _patientsRepository;

        public PatientsService(IPatientsRepository foodsRepository)
        {
            _patientsRepository = foodsRepository;
        }

        public PatientsDTO GetPatientById(int id)
        {
            var patients = _patientsRepository.GetById(id);
            if (patients == null)
            {
                return null;
            }

            return new PatientsDTO
            {
                Id = patients.Id,
                Nome = patients.Nome,
                Idade = patients.Idade,
                Genero = patients.Genero,
            };
        }

        public IEnumerable<PatientsDTO> GetAllPatients()
        {
            var patients = _patientsRepository.GetAll();

            return patients.Select(p => new PatientsDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade,
                Genero = p.Genero
            });
        }

        public Patients CreatePatient(PatientsDTO patientsDTO)
        {
            var patients = new Patients(patientsDTO.Nome, patientsDTO.Idade, patientsDTO.Genero, patientsDTO.DataCriacao);
            _patientsRepository.Create(patients);
            return patients;
        }



        public Patients UpdatePatient(PatientsDTO patientsDTO)
        {
            var patients = _patientsRepository.GetById(patientsDTO.Id);

            if (patients == null)
                throw new Exception("Paciente não encontrado.");

            patients.Nome = patientsDTO.Nome;
            patients.Genero = patientsDTO.Genero;
            patients.Idade = patientsDTO.Idade;

            _patientsRepository.Update(patients);

            return patients;
        }

        public void RemovePatient(int id)
        {
            _patientsRepository.Delete(id);
        }
    }
}
