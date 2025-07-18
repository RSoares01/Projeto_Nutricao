using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;

namespace Projeto_Nutri.Application.Service
{
    public class FoodsService
    {
        private readonly IFoodsRepository _foodsRepository;

        public FoodsService(IFoodsRepository foodsRepository)
        {
            _foodsRepository = foodsRepository;
        }

        public FoodsDTO GetFoodById(int id)
        {
            var food = _foodsRepository.GetById(id);
            if (food == null)
            {
                return null;
            }

            return new FoodsDTO
            {
                Id = food.Id,
                Nome = food.Nome,
            };
        }

        public IEnumerable<FoodsDTO> GetAllFoods()
        {
            var foods = _foodsRepository.GetAll();

            return foods.Select(f => new FoodsDTO
            {
                Id = f.Id,
                Nome = f.Nome,
            });
        }

        public Foods CreateFood(FoodsDTO foodsDTO)
        {
            var food = new Foods(foodsDTO.Nome, foodsDTO.Caloriaspor100g, foodsDTO.DataCriacao);
            _foodsRepository.Create(food);
            return food;
        }



        public Foods UpdateFood(FoodsDTO foodsDTO)
        {
            var food = _foodsRepository.GetById(foodsDTO.Id);

            if (food == null)
                throw new Exception("Alimento não encontrado.");

            food.Nome = foodsDTO.Nome;

            _foodsRepository.Update(food);

            return food;
        }

        public void RemoveFood(int id)
        {
            _foodsRepository.Delete(id);
        }
    }
}
