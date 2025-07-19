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

        // GET /patients?page=1&pageSize=10&search=joao
        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                List<PatientsDTO> allPatients = _service.GetAllPatients().ToList();

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
                return StatusCode(500, new { error = "Erro ao buscar pacientes.", details = ex.Message });
            }
        }

        // GET /patients/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var patient = _service.GetPatientById(id);
                if (patient == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado." });

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno. Tente novamente mais tarde.", details = ex.Message });
            }
        }

        // POST /patients (somente ADMIN)
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create([FromBody] PatientsDTO patients)
        {
            if (patients == null)
                return BadRequest(new { error = "Dados do paciente não podem ser nulos." });

            try
            {
                var createdPatient = _service.CreatePatient(patients);
                return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao criar o paciente.", details = ex.Message });
            }
        }

        // PUT /patients/{id} (somente ADMIN)
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Update(int id, [FromBody] PatientsDTO updatedPatient)
        {
            if (updatedPatient == null)
                return BadRequest(new { error = "Dados do paciente não podem ser nulos." });

            try
            {
                var result = _service.UpdatePatient(updatedPatient);
                if (result == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado." });

                return Ok(result);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao atualizar o paciente.", details = ex.Message });
            }
        }

        // DELETE /patients/{id} (somente ADMIN)
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingPatient = _service.GetPatientById(id);
                if (existingPatient == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado." });

                _service.RemovePatient(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao deletar o paciente.", details = ex.Message });
            }
        }
    }
}
