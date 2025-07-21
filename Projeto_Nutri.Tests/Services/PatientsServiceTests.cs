using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Infrastructure.IRepository;
using Moq;


namespace Projeto_Nutri.Tests
{
    public class PatientsServiceTests
    {
        private readonly Mock<IPatientsRepository> _mockRepo;
        private readonly PatientsService _service;

        public PatientsServiceTests()
        {
            _mockRepo = new Mock<IPatientsRepository>();
            _service = new PatientsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllPatients_ReturnsList()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Patients>
            {
                new Patients { Id = 1, Nome = "Ana" },
                new Patients { Id = 2, Nome = "Carlos" }
            });

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Nome == "Ana");
        }

        [Fact]
        public async Task GetPatientById_ReturnsCorrectPatient()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Patients { Id = 1, Nome = "João" });

            // Act
            var result = await _service.GetPatientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("João", result.Nome);
        }
    }
}
