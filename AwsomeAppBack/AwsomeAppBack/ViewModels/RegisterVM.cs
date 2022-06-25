using System.ComponentModel.DataAnnotations;

namespace AwsomeAppBack.ViewModels
{
    public class RegisterVM
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Login { get; set; }
        [Required,DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password),ErrorMessage ="Passwords not match"),MinLength(8)]
        public string ConfirmPassword { get; set; }
    }
}
