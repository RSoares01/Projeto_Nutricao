using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Entity;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("foods")]
    //[Authorize(Roles = "ADMIN,NUTRITIONIST")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodsService _service;

        public FoodsController(FoodsService service)
        {
            _service = service;
        }

        // GET /foods
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var foods = _service.GetAllFoods();
                if (foods == null || !foods.Any())
                    return NotFound(new { message = "Nenhum alimento encontrado." });

                return Ok(foods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno. Tente novamente mais tarde.", details = ex.Message });
            }
        }

        // GET /foods/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var food = _service.GetFoodById(id);
                if (food == null)
                    return NotFound(new { message = $"Alimento com ID {id} não encontrado." });

                return Ok(food);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno. Tente novamente mais tarde.", details = ex.Message });
            }
        }

        // POST /foods
        [HttpPost]
        public IActionResult Create([FromBody] FoodsDTO foods)
        {
            if (foods == null)
                return BadRequest(new { error = "Dados do alimento não podem ser nulos." });

            try
            {
                var createdFood = _service.CreateFood(foods);
                return CreatedAtAction(nameof(GetById), new { id = createdFood.Id }, createdFood);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao criar o alimento.", details = ex.Message });
            }
        }

        // PUT /foods/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FoodsDTO updatedFood)
        {
            if (updatedFood == null)
                return BadRequest(new { error = "Dados do alimento não podem ser nulos." });

            try
            {
                var result = _service.UpdateFood(updatedFood);
                if (result == null)
                    return NotFound(new { message = $"Alimento com ID {id} não encontrado." });

                return Ok(result);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao atualizar o alimento.", details = ex.Message });
            }
        }

        // DELETE /foods/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingFood = _service.GetFoodById(id);
                if (existingFood == null)
                    return NotFound(new { message = $"Alimento com ID {id} não encontrado." });

                _service.RemoveFood(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao deletar o alimento.", details = ex.Message });
            }
        }
    }
}
