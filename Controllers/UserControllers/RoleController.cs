using Livraria.Data;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.RoleControllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpGet("v1/roles")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
        {
            try
            {
                var roles = await context.Roles.ToListAsync();
                return Ok(new ResultViewModel<List<Role>>(roles));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao acessar servidor"));
            }
        }

        [HttpGet("v1/roles/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var role = await context.Roles.FindAsync(id);

                if (role is null)
                    return BadRequest(new ResultViewModel<Role>("40exU - Perfil não encontrado"));

                return Ok(new ResultViewModel<Role>(role));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpPost("v1/roles")]
        public async Task<IActionResult> Post([FromBody] RoleViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (model is null)
                    return BadRequest(new ResultViewModel<Role>("40exU - Perfil não existe"));

                var role = new Role
                {
                    Name = model.Name,
                    Slug = model.Name.ToLower()
                };

                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();

                return Created($"v1/roles/{role.Id}", new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao inserir perfil"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("v1/roles/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] RoleViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var role = await context.Roles.FindAsync(id);

                if (role is null)
                    return BadRequest(new ResultViewModel<Role>("40exU - Perfil não existe"));

                role.Name = model.Name;
                role.Slug = model.Name.ToLower();

                context.Roles.Update(role);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao atualizar perfil"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpDelete("v1/roles/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
        {
            try
            {
                var role = await context.Roles.FindAsync(id);

                if (role is null)
                    return BadRequest(new ResultViewModel<Role>("40exU - Perfil não existe"));

                context.Roles.Remove(role);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao excluir perfil"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("50exU - Erro ao acessar o servidor"));
            }
        }
    }
}
