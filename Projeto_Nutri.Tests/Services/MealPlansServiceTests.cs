using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;
using Moq;


namespace Projeto_Nutri.Tests
{
    public class MealPlansServiceTests
    {
        private readonly Mock<IMealPlansRepository> _mockMealPlanRepo;
        private readonly Mock<IPatientsRepository> _mockPatientsRepo;
        private readonly Mock<IFoodsRepository> _mockFoodsRepo;
        private readonly MealPlansService _service;

        public MealPlansServiceTests()
        {
            _mockMealPlanRepo = new Mock<IMealPlansRepository>();
            _mockPatientsRepo = new Mock<IPatientsRepository>();
            _mockFoodsRepo = new Mock<IFoodsRepository>();

            _service = new MealPlansService(
                _mockMealPlanRepo.Object,
                _mockPatientsRepo.Object,
                _mockFoodsRepo.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfMealPlans()
        {
            // Arrange
            _mockMealPlanRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<MealPlans>
            {
                new MealPlans { Id = 1, Nome = "Plano A", DataCriacao = DateTime.Today },
                new MealPlans { Id = 2, Nome = "Plano B", DataCriacao = DateTime.Today }
            });

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Nome == "Plano A");
        }
    }
}
