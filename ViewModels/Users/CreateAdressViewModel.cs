using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Users
{
    public class CreateAdressViewModel
    {
        [Required(ErrorMessage = "O campo id usuário é obrigatório")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "O campo número é obrigatório")]
        public int Number { get; set; }
        [Required(ErrorMessage = "O campo rua é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo rua precisa ser entre 3 e 100 caracteres")]
        public string Street { get; set; }
        [Required(ErrorMessage = "O campo bairro é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo bairro precisa ser entre 3 e 100 caracteres")]
        public string District { get; set; }
        [Required(ErrorMessage = "O campo cidade é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo cidade precisa ser entre 3 e 100 caracteres")]
        public string City { get; set; }
        [Required(ErrorMessage = "O campo estado é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo estado precisa ser entre 3 e 50 caracteres")]
        public string State { get; set; }
        [Required(ErrorMessage = "O campo cep é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo cep precisa ser entre 3 e 100 caracteres")]
        public string ZipCode { get; set; }
    }
}
