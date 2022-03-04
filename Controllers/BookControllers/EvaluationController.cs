using Livraria.Data;
using Livraria.Models.Books;
using Livraria.ViewModels;
using Livraria.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.BookControllers
{
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        [HttpGet("v1/evaluations")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var evaluations = await context.Evaluations.ToListAsync();
                return Ok(new ResultViewModel<List<Evaluation>>(evaluations));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Evaluation>("50exE - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("v1/evaluations/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
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

        [HttpPost("v1/evaluations")]
        public async Task<IActionResult> Post([FromBody] EvaluationViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
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

        [HttpPut("v1/evaluations/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] EvaluationViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
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

        [HttpDelete("v1/evaluations/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
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
