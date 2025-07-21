using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Nutri.Application.Service
{
    public class FoodsService
    {
        private readonly IFoodsRepository _foodsRepository;

        public FoodsService(IFoodsRepository foodsRepository)
        {
            _foodsRepository = foodsRepository;
        }

        public async Task<FoodsDTO?> GetFoodByIdAsync(int id)
        {
            var food = await _foodsRepository.GetByIdAsync(id);
            if (food == null)
                return null;

            return new FoodsDTO
            {
                Id = food.Id,
                Nome = food.Nome,
            };
        }

        public async Task<IEnumerable<FoodsDTO>> GetAllFoodsAsync()
        {
            var foods = await _foodsRepository.GetAllAsync();

            return foods.Select(f => new FoodsDTO
            {
                Id = f.Id,
                Nome = f.Nome,
            });
        }

        public async Task<Foods> CreateFoodAsync(FoodsDTO foodsDTO)
        {
            var food = new Foods(foodsDTO.Nome, foodsDTO.Caloriaspor100g, foodsDTO.DataCriacao);
            await _foodsRepository.CreateAsync(food);
            return food;
        }

        public async Task<Foods?> UpdateFoodAsync(FoodsDTO foodsDTO)
        {
            var food = await _foodsRepository.GetByIdAsync(foodsDTO.Id);
            if (food == null)
                return null;

            food.Nome = foodsDTO.Nome;
            await _foodsRepository.UpdateAsync(food);
            return food;
        }

        public async Task RemoveFoodAsync(int id)
        {
            await _foodsRepository.DeleteAsync(id);
        }
    }
}
