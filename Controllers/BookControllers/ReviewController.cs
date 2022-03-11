using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    [Authorize(Roles = "user")]
    [Route("v1/reviews")]
    public class ReviewController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var reviews = await context
                    .Reviews
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<List<Review>>(reviews));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var review = await context.Reviews.FindAsync(id);

                if (review is null)
                    return BadRequest(new ResultViewModel<Review>("40exE - Avaliação não existe"));

                return Ok(new ResultViewModel<Review>(review));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ReviewViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Review>(ModelState.GetErrors()));

                var review = new Review
                {
                    Stars = model.Stars,
                    Body = model.Body,
                    Title = model.Title,
                    UserId = model.UserId,
                    BookId = model.BookId,
                    Slug = Guid.NewGuid().ToString()
                };

                await context.Reviews.AddAsync(review);
                await context.SaveChangesAsync();

                return Created($"v1/reviews/{review.Id}", new ResultViewModel<Review>(review));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao inserir a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ReviewViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Review>(ModelState.GetErrors()));

                var review = await context.Reviews.FindAsync(id);

                if (review is null)
                    return BadRequest(new ResultViewModel<Review>("40exE - Avaliação não existe"));

                review.Title = model.Title;
                review.Body = model.Body;
                review.Stars = model.Stars;
                review.BookId = model.BookId;
                review.UserId = model.UserId;


                context.Reviews.Update(review);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Review>(review));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao atualizar a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var review = await context.Reviews.FindAsync(id);
                if (review is null)
                    return BadRequest(new ResultViewModel<Review>("40exE - avaliação não existe"));

                context.Reviews.Remove(review);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Review>(review));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao excluir a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exE - Erro ao acessar o servidor"));
            }
        }
    }
}
