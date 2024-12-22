using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VideoForum.ViewModels.Account
{
    public class Register_vm
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]      
        public string? Email { get; set; }


        [Display(Name="Name (Username)")]
        [Required(ErrorMessage ="Username is required")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Name must be at least {2}, and maximum {1} characters")]
        [RegularExpression("^[a-zA-Z0-9_.-]*$", ErrorMessage = "Name must contain only a-z A-Z 0-9 characters")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{6,15}$", ErrorMessage = "Password must contain at least one letter, at least one number, and be between 6-15 characters in length with no special characters.")]
        public string? Password { get; set; }


        [Required(ErrorMessage ="Password confirmation is required")]
        [Display(Name ="Confirm Password")]
        [Compare(nameof(Password),ErrorMessage ="Password mismatch")]
        public string? ConfirmPassword { get; set; }
    }
}
