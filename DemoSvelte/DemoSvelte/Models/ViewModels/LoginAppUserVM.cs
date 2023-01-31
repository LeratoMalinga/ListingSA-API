using System.ComponentModel.DataAnnotations;

namespace DemoSvelte.Models.ViewModels
{
    public class LoginAppUserVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
