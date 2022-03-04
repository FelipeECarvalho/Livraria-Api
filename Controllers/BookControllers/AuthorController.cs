using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    public class AuthorController : ControllerBase
    {
        [HttpGet("v1/authors")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var authors = await context.Authors.ToListAsync();

                return Ok(new ResultViewModel<List<Author>>(authors));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpGet("v1/authors/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var author = await context.Authors.FindAsync(id);

                if (author is null)
                    return BadRequest(new ResultViewModel<Author>("40exA - Autor não existe"));

                return Ok(new ResultViewModel<Author>(author));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpPost("v1/authors/")]
        public async Task<IActionResult> Post([FromBody] AuthorViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Author>(ModelState.GetErrors()));


                var author = new Author
                {
                    Name = model.Name,
                    Photo = model.Photo,
                    Summary = model.Summary,
                    Slug = Guid.NewGuid().ToString()
                };

                await context.Authors.AddAsync(author);
                await context.SaveChangesAsync();

                return Created($"v1/authors/{author.Id}", new ResultViewModel<Author>(author));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao inserir autor"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpPut("v1/authors/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AuthorViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Author>(ModelState.GetErrors()));

                var author = await context.Authors.FindAsync(id);

                if (author is null)
                    return BadRequest(new ResultViewModel<Author>("40exA - Autor não existe"));


                author.Name = model.Name;
                author.Photo = model.Photo;
                author.Summary = model.Summary;

                context.Authors.Update(author);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Author>(author));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao atualizar o autor"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao acessar servidor"));
            }
        }


        [HttpDelete("v1/authors/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var author = await context.Authors.FindAsync(id);

                if (author is null)
                    return BadRequest(new ResultViewModel<Author>("40exA - Autor não existe"));

                context.Authors.Remove(author);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Author>(author));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao excluir o autor"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Author>("50exA - Erro ao acessar servidor"));
            }
        }
    }
}
