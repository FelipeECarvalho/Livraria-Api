using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Users;
using Livraria.ViewModels;
using Livraria.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;
using SecureIdentity.Password;
using Microsoft.EntityFrameworkCore;
using Livraria.Services;
using Microsoft.AspNetCore.Authorization;

namespace Livraria.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/accounts")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] LivrariaDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<dynamic>(ModelState.GetErrors()));

            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Slug = model.Email.ToLower().Replace('@', '-').Replace('.', '-')
            };

            var role = await context.Roles.FirstOrDefaultAsync(x => x.Slug == "user");
            if (role is null)
                throw new Exception();

            user.Roles.Add(role);

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new {user.Email, password}));

            }
            catch (DbUpdateException) 
            {
                return StatusCode(500, new ResultViewModel<User>("Email já cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("Não foi possível acessar servidor"));
            }
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] LivrariaDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));
            try
            {
                var user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user is null)
                    return StatusCode(401, new ResultViewModel<User>("Usuário ou senha inválidos"));

                if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                    return StatusCode(401, new ResultViewModel<User>("Usuário ou senha inválidos"));


                var token = new TokenService().GenerateToken(user);

                return Ok(token);
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("Não foi possível acessar servidor"));
            }
        }
    }
}



