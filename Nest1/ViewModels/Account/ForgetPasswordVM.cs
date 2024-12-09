using System.ComponentModel.DataAnnotations;

namespace Nest1.ViewModels.Account
{
    public class ForgetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
