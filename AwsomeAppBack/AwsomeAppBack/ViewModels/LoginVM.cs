using System.ComponentModel.DataAnnotations;

namespace AwsomeAppBack.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Login { get; set; }
        [Required,DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}
