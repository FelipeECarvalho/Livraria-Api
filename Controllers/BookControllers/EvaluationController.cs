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
    [Route("v1/evaluations")]
    public class EvaluationController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var evaluations = await context
                    .Evaluations
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<List<Evaluation>>(evaluations));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var evaluation = await context.Evaluations.FindAsync(id);

                if (evaluation is null)
                    return BadRequest(new ResultViewModel<Evaluation>("40exE - Avaliação não existe"));

                return Ok(new ResultViewModel<Evaluation>(evaluation));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] EvaluationViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Evaluation>(ModelState.GetErrors()));

                var evaluation = new Evaluation
                {
                    Rating = model.Rating,
                    Body = model.Body,
                    Title = model.Title,
                    UserId = model.UserId,
                    BookId = model.BookId,
                    Slug = Guid.NewGuid().ToString()
                };

                await context.Evaluations.AddAsync(evaluation);
                await context.SaveChangesAsync();

                return Created($"v1/Evaluations/{evaluation.Id}", new ResultViewModel<Evaluation>(evaluation));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao inserir a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EvaluationViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Evaluation>(ModelState.GetErrors()));

                var evaluation = await context.Evaluations.FindAsync(id);

                if (evaluation is null)
                    return BadRequest(new ResultViewModel<Evaluation>("40exE - Avaliação não existe"));

                evaluation.Title = model.Title;
                evaluation.Body = model.Body;
                evaluation.Rating = model.Rating;
                evaluation.BookId = model.BookId;
                evaluation.UserId = model.UserId;


                context.Evaluations.Update(evaluation);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Evaluation>(evaluation));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao atualizar a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var evaluation = await context.Evaluations.FindAsync(id);
                if (evaluation is null)
                    return BadRequest(new ResultViewModel<Evaluation>("40exE - avaliação não existe"));

                context.Evaluations.Remove(evaluation);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Evaluation>(evaluation));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao excluir a avaliação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }
    }
}
