using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO.Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("mealplans")]
    //[Authorize(Roles = "ADMIN,NUTRITIONIST")]
    [ApiController]
    public class MealPlansController : ControllerBase
    {
        private readonly MealPlansService _service;

        public MealPlansController(MealPlansService service)
        {
            _service = service;
        }

        // GET /mealplans
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var plans = _service.GetAll();
                if (plans == null || !plans.Any())
                    return NotFound(new { message = "Nenhum plano alimentar encontrado." });

                return Ok(plans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao buscar planos alimentares.", details = ex.Message });
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

        // POST /mealplans
        [HttpPost]
        public IActionResult Create([FromBody] MealPlanCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Dados do plano alimentar não podem ser nulos." });

            try
            {
                var created = _service.Create(dto);
                var createdDto = _service.GetById(created.Id); // usa DTO para evitar o ciclo

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


        // DELETE /mealplans/{id}
        [HttpDelete("{id}")]
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


        [HttpGet("/patients/{Idpatient}/mealplans/today")]
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
