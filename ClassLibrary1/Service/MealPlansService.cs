using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.DTO.Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;

namespace Projeto_Nutri.Application.Service
{
    public class MealPlansService
    {
        private readonly IMealPlansRepository _mealPlansRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly IFoodsRepository _foodsRepository;

        public MealPlansService(
            IMealPlansRepository mealPlansRepository,
            IPatientsRepository patientsRepository,
            IFoodsRepository foodsRepository)
        {
            _mealPlansRepository = mealPlansRepository;
            _patientsRepository = patientsRepository;
            _foodsRepository = foodsRepository;
        }

        public async Task<MealPlansDTO?> GetByIdAsync(int id)
        {
            var plan = await _mealPlansRepository.GetByIdAsync(id);
            if (plan == null)
                return null;

            var dto = new MealPlansDTO
            {
                Id = plan.Id,
                Nome = plan.Nome,
                DataCriacao = plan.DataCriacao,
                PatientId = plan.PatientId,
                NomeDoPaciente = plan.Patient?.Nome ?? string.Empty,
                Alimentos = plan.Alimentos.Select(a => new MealPlanFoodReadDTO
                {
                    FoodId = a.FoodId,
                    Nome = a.Food?.Nome ?? string.Empty,
                    CaloriasPor100g = a.Food?.CaloriasPor100g ?? 0,
                    TamanhoDaPorcaoEmGramas = a.TamanhoDaPorcaoEmGramas
                }).ToList()
            };

            dto.CaloriasTotais = dto.Alimentos.Sum(a => a.CaloriasTotais);
            return dto;
        }

        public async Task<IEnumerable<MealPlansDTO>> GetAllAsync()
        {
            var plans = await _mealPlansRepository.GetAllAsync();

            return plans.Select(p => new MealPlansDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                DataCriacao = p.DataCriacao,
                NomeDoPaciente = p.Patient?.Nome ?? string.Empty
            });
        }

        public async Task<MealPlans> CreateAsync(MealPlanCreateDTO dto)
        {
            var patient = await _patientsRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                throw new Exception("Paciente não encontrado.");

            var mealPlan = new MealPlans(dto.PatientId, dto.Nome, DateTime.Now);

            foreach (var item in dto.Alimentos)
            {
                var food = await _foodsRepository.GetByIdAsync(item.FoodId);
                if (food == null)
                    throw new Exception($"Alimento com ID {item.FoodId} não encontrado.");

                mealPlan.Alimentos.Add(new MealPlanFoods
                {
                    FoodId = item.FoodId,
                    TamanhoDaPorcaoEmGramas = item.TamanhoDaPorcaoEmGramas,
                    MealPlan = mealPlan
                });
            }

            await _mealPlansRepository.CreateAsync(mealPlan);
            return mealPlan;
        }

        public async Task DeleteAsync(int id)
        {
            await _mealPlansRepository.DeleteAsync(id);
        }

        public async Task<List<MealPlansDTO>> GetTodayMealPlansByPatientIdAsync(int patientId)
        {
            var allPlans = await _mealPlansRepository.GetAllAsync();
            var plans = allPlans
                .Where(p => p.PatientId == patientId && p.DataCriacao.Date == DateTime.Today)
                .ToList();

            if (!plans.Any())
                return new List<MealPlansDTO>();

            var dtos = plans.Select(plan => new MealPlansDTO
            {
                Id = plan.Id,
                Nome = plan.Nome,
                DataCriacao = plan.DataCriacao,
                PatientId = plan.PatientId,
                NomeDoPaciente = plan.Patient?.Nome ?? string.Empty,
                Alimentos = plan.Alimentos.Select(a => new MealPlanFoodReadDTO
                {
                    FoodId = a.FoodId,
                    Nome = a.Food?.Nome ?? string.Empty,
                    CaloriasPor100g = a.Food?.CaloriasPor100g ?? 0,
                    TamanhoDaPorcaoEmGramas = a.TamanhoDaPorcaoEmGramas
                }).ToList(),
                CaloriasTotais = plan.Alimentos.Sum(a =>
                    Math.Round(((a.Food?.CaloriasPor100g ?? 0) / 100m) * a.TamanhoDaPorcaoEmGramas, 2))
            }).ToList();

            return dtos;
        }
    }
}
