using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "image is required")]
        public string Base64Image { get; set; }
    }
}
