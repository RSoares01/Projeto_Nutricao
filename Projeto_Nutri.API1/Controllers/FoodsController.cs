using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Exceptions;

namespace Projeto_Nutri.API.Controllers
{
    [Route("foods")]
    [ApiController]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public class FoodsController : ControllerBase
    {
        private readonly FoodsService _service;

        public FoodsController(FoodsService service)
        {
            _service = service;
        }

        // GET /foods?page=1&pageSize=10&search=banana
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                var allFoods = (await _service.GetAllFoodsAsync()).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                    allFoods = allFoods
                        .Where(f => f.Nome.ToLower().Contains(search.ToLower()))
                        .ToList();

                var totalItems = allFoods.Count;
                var pagedFoods = allFoods
                    .OrderBy(f => f.Nome)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var response = new
                {
                    currentPage = page,
                    pageSize,
                    totalItems,
                    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    items = pagedFoods
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar alimentos.", details = ex.Message });
            }
        }

        // GET /foods/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var food = await _service.GetFoodByIdAsync(id);
                if (food == null)
                    return NotFound(new { message = $"Alimento com ID {id} não encontrado." });

                return Ok(food);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno.", details = ex.Message });
            }
        }

        // POST /foods
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] FoodsDTO foods)
        {
            if (foods == null)
                return BadRequest(new { error = "Dados do alimento não podem ser nulos." });

            try
            {
                var createdFood = await _service.CreateFoodAsync(foods);
                return CreatedAtAction(nameof(GetById), new { id = createdFood.Id }, createdFood);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao criar o alimento.", details = ex.Message });
            }
        }

        // PUT /foods
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] FoodsDTO updatedFood)
        {
            if (updatedFood == null)
                return BadRequest(new { error = "Dados do alimento não podem ser nulos." });

            try
            {
                var result = await _service.UpdateFoodAsync(updatedFood);
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
                return StatusCode(500, new { error = "Erro ao atualizar o alimento.", details = ex.Message });
            }
        }

        // DELETE /foods/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingFood = await _service.GetFoodByIdAsync(id);
                if (existingFood == null)
                    return NotFound(new { message = $"Alimento com ID {id} não encontrado." });

                await _service.RemoveFoodAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao deletar o alimento.", details = ex.Message });
            }
        }
    }
}
