using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.DTO.Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("mealplans")]
    [ApiController]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public class MealPlansController : ControllerBase
    {
        private readonly MealPlansService _service;

        public MealPlansController(MealPlansService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                var allPlans = (await _service.GetAllAsync()).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                    allPlans = allPlans
                        .Where(p => p.Nome.ToLower().Contains(search.ToLower()))
                        .ToList();

                var totalItems = allPlans.Count;
                var pagedPlans = allPlans
                    .OrderBy(p => p.Nome)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var response = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalItems,
                    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    items = pagedPlans
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar planos alimentares.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var plan = await _service.GetByIdAsync(id);
                if (plan == null)
                    return NotFound(new { message = $"Plano alimentar com ID {id} não encontrado." });

                return Ok(plan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao buscar o plano alimentar.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] MealPlanCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Dados do plano alimentar não podem ser nulos." });

            try
            {
                var created = await _service.CreateAsync(dto);
                var createdDto = await _service.GetByIdAsync(created.Id);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao criar o plano alimentar.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Plano alimentar com ID {id} não encontrado." });

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao excluir o plano alimentar.", details = ex.Message });
            }
        }

        [HttpGet("/patients/{id}/mealplans/today")]
        public async Task<IActionResult> GetTodayByPatientId(int id)
        {
            try
            {
                var plans = await _service.GetTodayMealPlansByPatientIdAsync(id);

                if (plans == null || !plans.Any())
                    return NotFound(new { message = $"Nenhum plano encontrado para hoje para o paciente {id}." });

                return Ok(plans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar os planos de hoje.", details = ex.Message });
            }
        }
    }
}
