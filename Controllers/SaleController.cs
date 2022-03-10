using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models;
using Livraria.ViewModels;
using Livraria.ViewModels.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers
{
    [ApiController]
    [Route("v1/sales")]
    [Authorize(Roles = "user,administrator")]
    public class SaleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context)
        {
            try
            {
                var sales = await context.Sales.ToListAsync();

                return Ok(new ResultViewModel<List<Sale>>(sales));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var sale = await context.Sales.FindAsync(id);

                if (sale is null)
                    return BadRequest(new ResultViewModel<Sale>("40exS - Venda não existe"));

                return Ok(new ResultViewModel<Sale>(sale));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao acessar o servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateSaleViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Sale>(ModelState.GetErrors()));

                var user = await context.Users.FindAsync(model.UserId);

                if (user is null)
                    return BadRequest(new ResultViewModel<Sale>("40exS - Usuário não existe"));

                var sale = new Sale
                {
                    Date = model.Date,
                    Status = 0,
                    User = user
                };

                foreach (var id in model.BooksId)
                {
                    var book = await context.Books.FindAsync(id);
                    if (book is null)
                        return BadRequest(new ResultViewModel<Sale>("40exS - Livro ou livros não existem"));
                    sale.Books.Add(book);
                }

                sale.Value = sale.Books.Sum(x => x.Price);

                await context.Sales.AddAsync(sale);
                await context.SaveChangesAsync();

                return Created($"v1/sales/{sale.Id}", new ResultViewModel<Sale>(sale));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao inserir a venda"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateSaleViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Sale>(ModelState.GetErrors()));

                var sale = await context.Sales.FindAsync(id);

                if (sale is null)
                    return BadRequest(new ResultViewModel<Sale>("40exS - Venda não existe"));

                var item = context.Entry(sale);

                item.State = EntityState.Modified;

                item.Collection(x => x.Books).Load();

                sale.Books.Clear();


                foreach (var value in model.BooksId)
                {
                    var book = await context.Books.FindAsync(value);
                    if (book is null)
                        return BadRequest(new ResultViewModel<Sale>("40exS - Livro ou livros não existem"));
                    sale.Books.Add(book);
                }

                sale.Status = model.Status;
                sale.Value = sale.Books.Sum(x => x.Price);

                context.Sales.UpdateRange(sale);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Sale>(sale));

            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao atualizar a venda"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var sale = await context.Sales.FindAsync(id);

                if (sale is null)
                    return BadRequest("40exS - Venda não existe");

                context.Sales.Remove(sale);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Sale>(sale));

            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao excluir a venda"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Sale>("50exS - Erro ao acessar o servidor"));
            }
        }
    }
}
