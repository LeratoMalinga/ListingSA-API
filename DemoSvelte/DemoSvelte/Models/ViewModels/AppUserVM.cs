using System.ComponentModel.DataAnnotations;

namespace DemoSvelte.Models.ViewModels
{
    public class AppUserVM
    {
        [Required ,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
