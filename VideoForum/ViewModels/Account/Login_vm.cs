using System.ComponentModel.DataAnnotations;

namespace VideoForum.ViewModels.Account
{
    public class Login_vm
    {
        private string? _userName;

        //case-insensitive username
        [Display(Name ="Username or Email")]
        [Required(ErrorMessage ="Username is required")]
        public string? UserName { 
            get => _userName; 
            set => _userName = string.IsNullOrWhiteSpace(value) is false ?
                value?.ToLower():
                null;
        }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
