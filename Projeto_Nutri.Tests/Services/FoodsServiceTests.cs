using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;

namespace Projeto_Nutri.Tests.Services
{
    public class FoodsServiceTests
    {
        private readonly Mock<IFoodsRepository> _mockRepo;
        private readonly FoodsService _service;

        public FoodsServiceTests()
        {
            _mockRepo = new Mock<IFoodsRepository>();
            _service = new FoodsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllFoods_ReturnsListOfFoods()
        {
            // Arrange
            var foods = new List<Foods>
            {
                new Foods { Id = 1, Nome = "Arroz" },
                new Foods { Id = 2, Nome = "Feijão" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(foods);

            // Act
            var result = await _service.GetAllFoodsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.Nome == "Arroz");
            Assert.Contains(result, f => f.Nome == "Feijão");
        }

        [Fact]
        public async Task GetFoodById_ExistingId_ReturnsFood()
        {
            // Arrange
            var food = new Foods { Id = 10, Nome = "Banana" };
            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(food);

            // Act
            var result = await _service.GetFoodByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Banana", result.Nome);
        }

        [Fact]
        public async Task GetFoodById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Foods)null);

            // Act
            var result = await _service.GetFoodByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}
