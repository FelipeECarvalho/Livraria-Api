using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                var categories = await cache.GetOrCreateAsync("CategoryCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategoriesAsync(context);
                });
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("50exB - Erro ao acessar o servidor"));
            }
        }

        private async Task<List<Category>> GetCategoriesAsync(LivrariaDataContext context)
            => await context.Categories.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
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

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> PostAsync([FromBody] CategoryViewModel model, [FromServices] LivrariaDataContext context)
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

        [HttpPut("{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] CategoryViewModel model, [FromServices] LivrariaDataContext context)
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

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
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
