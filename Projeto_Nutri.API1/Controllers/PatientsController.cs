using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("patients")]
    [ApiController]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsService _service;

        public PatientsController(PatientsService service)
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

                var allPatients = (await _service.GetAllPatientsAsync()).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                    allPatients = allPatients
                        .Where(p => p.Nome.ToLower().Contains(search.ToLower()))
                        .ToList();

                var totalItems = allPatients.Count;
                var pagedPatients = allPatients
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
                    items = pagedPatients
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Erro ao buscar pacientes."
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var patient = await _service.GetPatientByIdAsync(id);
                if (patient == null)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Paciente não encontrado.",
                        Detail = $"Paciente com ID {id} não foi localizado.",
                        Status = 404
                    });

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Erro ao buscar paciente."
                );
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] PatientsDTO patients)
        {
            if (patients == null)
                return BadRequest(new ProblemDetails
                {
                    Title = "Dados inválidos.",
                    Detail = "Dados do paciente não podem ser nulos.",
                    Status = 400
                });

            try
            {
                var createdPatient = await _service.CreatePatientAsync(patients);
                return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
            }
            catch (DomainException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Erro de domínio.",
                    Detail = ex.Message,
                    Status = 400
                });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Erro ao criar paciente."
                );
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientsDTO updatedPatient)
        {
            if (updatedPatient == null)
                return BadRequest(new ProblemDetails
                {
                    Title = "Dados inválidos.",
                    Detail = "Dados do paciente não podem ser nulos.",
                    Status = 400
                });

            try
            {
                var result = await _service.UpdatePatientAsync(updatedPatient);
                if (result == null)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Paciente não encontrado.",
                        Detail = $"Paciente com ID {id} não foi localizado.",
                        Status = 404
                    });

                return Ok(result);
            }
            catch (DomainException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Erro de domínio.",
                    Detail = ex.Message,
                    Status = 400
                });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Erro ao atualizar paciente."
                );
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingPatient = await _service.GetPatientByIdAsync(id);
                if (existingPatient == null)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Paciente não encontrado.",
                        Detail = $"Paciente com ID {id} não foi localizado.",
                        Status = 404
                    });

                await _service.RemovePatientAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Erro ao deletar paciente."
                );
            }
        }
    }
}
