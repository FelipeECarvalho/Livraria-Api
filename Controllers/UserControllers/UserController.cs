﻿using Livraria.Data;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers.UserControllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("v1/users")]
        public async Task<IActionResult> Get([FromServices] LivrariaDataContext context)
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

        [HttpGet("v1/users/{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] LivrariaDataContext context)
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

        [HttpPost("v1/users")]
        public async Task<IActionResult> Post([FromBody] UserViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
                if (model is null)
                    return BadRequest(new ResultViewModel<User>("40exU - Usuário não existe"));

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    PasswordHash = model.PasswordHash,
                    Slug = Guid.NewGuid().ToString(),
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"v1/users/{user.Id}", new ResultViewModel<User>(user));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao inserir usuario"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("50exU - Erro ao acessar o servidor"));
            }
        }

        [HttpPut("v1/users/{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UserViewModel model, [FromServices] LivrariaDataContext context)
        {
            try
            {
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

        [HttpDelete("v1/users/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] LivrariaDataContext context)
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
