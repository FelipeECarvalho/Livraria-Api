using Livraria.Data;
using Livraria.Extensions;
using Livraria.Models.Users;
using Livraria.Services;
using Livraria.ViewModels;
using Livraria.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Livraria.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/accounts")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model,[FromServices]EmailService emailService, [FromServices] LivrariaDataContext context)
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

                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("Não foi possível acessar servidor"));
            }
        }

        [Authorize]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromBody] UploadImageViewModel model, [FromServices] LivrariaDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var fileName = Guid.NewGuid().ToString() + ".jpg";

            var data = new Regex(@"^data:image\/[a-z]+;base64,")
                .Replace(model.Base64Image, "");

            var imageBytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/users/{fileName}", imageBytes);
            }
            catch (IOException)
            {
                return StatusCode(500, new ResultViewModel<string>("50exAc - Erro ao inserir imagem"));
            }

            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user is null)
                return BadRequest(new ResultViewModel<User>("40exAc - Usuário inválido"));

            user.Image = $"https://localhost:0000/images/users/{fileName}";

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Imagem inserida com sucesso!", null));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<User>("50exAc - Erro ao inserir imagem"));
            }
        }
    }
}



