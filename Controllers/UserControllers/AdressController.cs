using Livraria.Data;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.UserControllers
{
    [ApiController]
    public class AdressController : ControllerBase
    {
        [HttpGet("v1/adresses")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var adresses = await context.Adresses.ToListAsync();
                return Ok(new ResultViewModel<List<Adress>>(adresses));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpGet("v1/adresses/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var adress = await context.Adresses.FindAsync(id);

                if (adress is null)
                    return BadRequest(new ResultViewModel<Adress>("40exA - Endereço não encontrado"));

                return Ok(new ResultViewModel<Adress>(adress));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Erro ao acessar o servidor"));
            }
        }

        [HttpPost("v1/adresses")]
        public async Task<IActionResult> Post([FromBody] CreateAdressViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (model is null)
                    return BadRequest(new ResultViewModel<Adress>("40exA - Endereço inválido"));

                var adress = new Adress
                {
                    City = model.City,
                    Street = model.Street,
                    District = model.District,
                    Number = model.Number,
                    State = model.State,
                    UserId = model.UserId,
                    ZipCode = model.ZipCode,
                    Slug = Guid.NewGuid().ToString()
                };

                await context.Adresses.AddAsync(adress);
                await context.SaveChangesAsync();

                return Created($"v1/adresses/{adress.Id}", new ResultViewModel<Adress>(adress));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Erro ao inserir endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpPut("v1/adresses/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateAdressViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var adress = await context.Adresses.FindAsync(id);

                if (adress is null)
                    return BadRequest(new ResultViewModel<Adress>("40exA - Endereço não existe"));

                adress.Street = model.Street;
                adress.District = model.District;
                adress.Number = model.Number;
                adress.State = model.State;
                adress.ZipCode = model.ZipCode;
                adress.City = model.City;

                context.Adresses.Update(adress);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Adress>(adress));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Não foi possível atualizar o endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Não foi possível acessar o servidor"));
            }
        }

        [HttpDelete("v1/adresses/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var adress = await context.Adresses.FindAsync(id);

                if (adress is null)
                    return BadRequest(new ResultViewModel<Adress>("40exA - Endereço não existe"));

                context.Adresses.Remove(adress);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Adress>(adress));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Adress>("40exA - Não foi possível deletar o endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Adress>("50exA - Erro ao acessar o servidor"));
            }
        }
    }
}
