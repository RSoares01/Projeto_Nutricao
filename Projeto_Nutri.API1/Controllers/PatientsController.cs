using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("patients")]
    //[Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public class PatientsController : Controller
    {
        private readonly PatientsService _service;

        public PatientsController(PatientsService service)
        {
            _service = service;
        }
        // GET /patients
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var patients = _service.GetAllPatients();
                if (patients == null || !patients.Any())
                    return NotFound(new { message = "Nenhum Paciente encontrado." });

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno. Tente novamente mais tarde.", details = ex.Message });
            }
        }

        // GET /patients/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var patients = _service.GetPatientById(id);
                if (patients == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado." });

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno. Tente novamente mais tarde.", details = ex.Message });
            }
        }

        // POST /patients
        [HttpPost]
        public IActionResult Create([FromBody] PatientsDTO patients)
        {
            if (patients == null)
                return BadRequest(new { error = "Dados do Paceinte não podem ser nulos." });

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

        // PUT /patients/{id}
        [HttpPut()]
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

        // DELETE /patients/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingPacient = _service.GetPatientById(id);
                if (existingPacient == null)
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
