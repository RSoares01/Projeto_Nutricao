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
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                List<MealPlansDTO> allPlans = _service.GetAll().ToList();

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



        // GET /mealplans/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var plan = _service.GetById(id);
                if (plan == null)
                    return NotFound(new { message = $"Plano alimentar com ID {id} não encontrado." });

                return Ok(plan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao buscar o plano alimentar.", details = ex.Message });
            }
        }

        // POST /mealplans (somente ADMIN)
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create([FromBody] MealPlanCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Dados do plano alimentar não podem ser nulos." });

            try
            {
                var created = _service.Create(dto);
                var createdDto = _service.GetById(created.Id); // evita retorno incompleto

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

        // DELETE /mealplans/{id} (somente ADMIN)
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existing = _service.GetById(id);
                if (existing == null)
                    return NotFound(new { message = $"Plano alimentar com ID {id} não encontrado." });

                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao excluir o plano alimentar.", details = ex.Message });
            }
        }

        // GET /patients/{Idpatient}/mealplans/today
        [HttpGet("/patients/{id}/mealplans/today")]
        public IActionResult GetTodayByPatientId(int id)
        {
            try
            {
                var plans = _service.GetTodayMealPlansByPatientId(id);

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
