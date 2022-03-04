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
    public class BookController : ControllerBase
    {
        [HttpGet("v1/books")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var books = await context.Books.ToListAsync();

                return Ok(new ResultViewModel<List<Book>>(books));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("v1/books/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var book = await context.Books.FindAsync(id);

                if (book is null)
                    return BadRequest(new ResultViewModel<Book>("40exB - Livro não existe"));

                return Ok(new ResultViewModel<Book>(book));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpPost("v1/books")]
        public async Task<IActionResult> Post([FromBody] CreateBookViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Book>(ModelState.GetErrors()));

                var book = new Book
                {
                    AuthorId = model.AuthorId,
                    CategoryId = model.CategoryId,
                    CreatedDate = model.CreatedDate,
                    Image = model.Image,
                    Language = model.Language,
                    PagesNumber = model.PagesNumber,
                    Summary = model.Summary,
                    Price = model.Price,
                    Title = model.Title,
                    Slug = model.Title.Replace(' ', '-').ToLower()
                };

                await context.Books.AddAsync(book);
                await context.SaveChangesAsync();

                return Created($"v1/books/{book.Id}", new ResultViewModel<Book>(book));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao inserir o livro"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("v1/books/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateBookViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Book>(ModelState.GetErrors()));

                var book = await context.Books.FindAsync(id);

                if (book is null)
                    return BadRequest(new ResultViewModel<Book>("40exB - Livro não existe"));

                book.Title = model.Title;
                book.CreatedDate = model.CreatedDate;
                book.Image = model.Image;
                book.PagesNumber = model.PagesNumber;
                book.Price = model.Price;
                book.Language = model.Language;
                book.Summary = model.Summary;

                context.Books.Update(book);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Book>(book));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao criar o livro"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("v1/books/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var book = await context.Books.FindAsync(id);

                if (book is null)
                    return BadRequest(new ResultViewModel<Book>("40exB - Livro não existe"));

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Book>(book));

            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao excluir o livro"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }
    }
}
