using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.UserControllers
{
    [ApiController]
    [Route("v1/addresses")]
    [Authorize(Roles = "user,administrator")]
    public class AddressController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context)
        {
            try
            {
                var addresses = await context
                    .Addresses
                    .ToListAsync();
                return Ok(new ResultViewModel<List<Address>>(addresses));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var address = await context.Addresses.FindAsync(id);

                if (address is null)
                    return BadRequest(new ResultViewModel<Address>("40exA - Endereço não encontrado"));

                return Ok(new ResultViewModel<Address>(address));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Erro ao acessar o servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAdressViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Address>(ModelState.GetErrors()));

                var address = new Address
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

                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();

                return Created($"v1/adresses/{address.Id}", new ResultViewModel<Address>(address));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Erro ao inserir endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Erro ao acessar servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateAdressViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Address>(ModelState.GetErrors()));

                var address = await context.Addresses.FindAsync(id);

                if (address is null)
                    return BadRequest(new ResultViewModel<Address>("40exA - Endereço não existe"));

                address.Street = model.Street;
                address.District = model.District;
                address.Number = model.Number;
                address.State = model.State;
                address.ZipCode = model.ZipCode;
                address.City = model.City;

                context.Addresses.Update(address);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Address>(address));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Não foi possível atualizar o endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Não foi possível acessar o servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var address = await context.Addresses.FindAsync(id);

                if (address is null)
                    return BadRequest(new ResultViewModel<Address>("40exA - Endereço não existe"));

                context.Addresses.Remove(address);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Address>(address));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Address>("40exA - Não foi possível deletar o endereço"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exA - Erro ao acessar o servidor"));
            }
        }
    }
}
