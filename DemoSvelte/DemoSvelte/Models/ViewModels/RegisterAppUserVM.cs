using System.ComponentModel.DataAnnotations;

namespace DemoSvelte.Models.ViewModels
{
    public class RegisterAppUserVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password),ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty.ToString();
    }
}
