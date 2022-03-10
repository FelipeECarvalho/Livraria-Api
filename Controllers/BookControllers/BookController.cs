using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    [Route("v1/books")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var books = await context
                    .Books
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<List<Book>>(books));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync([FromRoute] string category, [FromServices] LivrariaDataContext context, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var books = await context
                    .Books
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Where(x => x.Category.Slug == category)
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<List<Book>>(books));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Book>("50exB - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
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

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> PostAsync([FromBody] CreateBookViewModel model, [FromServices] LivrariaDataContext context)
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

        [HttpPut("{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateBookViewModel model, [FromServices] LivrariaDataContext context)
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

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
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

        [HttpPost("upload-image/{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> UploadImageAsync([FromRoute] int id, [FromBody] UploadImageViewModel model, [FromServices] LivrariaDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var book = await context.Books.FindAsync(id);

            if (book is null)
                return BadRequest(new ResultViewModel<string>("40exB - Livro não existe"));

            var fileName = Guid.NewGuid().ToString() + ".jpg";

            var data = new Regex(@"^data:image\/[a-z]+;base64,")
                .Replace(model.Base64Image, "");

            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/books/{fileName}", bytes);
            }
            catch (IOException)
            {
                return BadRequest(new ResultViewModel<string>("40exB - Erro ao inserir a imagem"));
            }

            book.Image = $"https://localhost:0000/images/books/{fileName}";

            try
            {
                context.Books.Update(book);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Imagem inserida com sucesso!", null));
            }
            catch (IOException)
            {
                return BadRequest(new ResultViewModel<string>("40exB - Erro ao inserir a imagem"));
            }
        }
    }
}
