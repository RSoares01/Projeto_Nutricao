using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Nutri.Application.DTO;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Domain.Entity;
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
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                List<FoodsDTO> allFoods = _service.GetAllFoods().ToList();

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
                    pageSize = pageSize,
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
        [Authorize(Roles = "ADMIN")]
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

        // PUT /foods
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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
