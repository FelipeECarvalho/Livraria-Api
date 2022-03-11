using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Books;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.UserControllers
{
    [ApiController]
    [Route("v1/users")]
    [Authorize(Roles = "user,administrator")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] LivrariaDataContext context)
        {
            try
            {
                var users = await context.Users.ToListAsync();
                return Ok(new ResultViewModel<List<User>>(users));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao acessar servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var user = await context.Users.FindAsync(id);

                if (user is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Usuário não encontrado"));

                return Ok(new ResultViewModel<User>(user));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddressAsync([FromServices] LivrariaDataContext context)
        {
            try
            {
                var adress = await context.Addresses.FirstOrDefaultAsync(x => x.User.Email == User.Identity.Name);

                if (adress is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Endereço não encontrado"));

                return Ok(new ResultViewModel<Address>(adress));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Address>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpGet("evaluation")]
        public async Task<IActionResult> GetReviewsAsync([FromServices] LivrariaDataContext context)
        {
            try
            {
                var reviews = await context.Reviews.Where(x => x.User.Email == User.Identity.Name).ToListAsync();

                if (reviews is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Avaliações não encontradas"));

                return Ok(new ResultViewModel<List<Review>>(reviews));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Review>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UserViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));

                var user = await context.Users.FindAsync(id);

                if (user is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Usuário não existe"));

                user.Name = model.Name;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.PasswordHash = model.PasswordHash;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<User>(user));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao atualizar usuario"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var user = await context.Users.FindAsync(id);

                if (user is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Usuário não existe"));

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<User>(user));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao excluir usuario"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao acessar o servidor"));
            }
        }
    }
}
