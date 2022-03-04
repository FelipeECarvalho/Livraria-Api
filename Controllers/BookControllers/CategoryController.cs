﻿using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var category = await context.Categories.FindAsync(id);

                if (category is null)
                    return BadRequest(new ResultViewModel<Category>("40exB - Categoria não existe"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> Post([FromBody] CategoryViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Name.Replace(' ', '-').ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao inserir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CategoryViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

                var category = await context.Categories.FindAsync(id);

                if (category is null)
                    return BadRequest(new ResultViewModel<Category>("40exB - Categoria não existe"));

                category.Name = model.Name;
                category.Slug = category.Name.Replace(' ', '-').ToLower();


                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao atualizar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var category = await context.Categories.FindAsync(id);
                if (category is null)
                    return BadRequest(new ResultViewModel<Category>("40exB - Categoria não existe"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao excluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }
    }
}