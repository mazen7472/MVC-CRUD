using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterViewModel
    {
        public string FName { get; set; }
        public string LName { get; set; }

        [Required(ErrorMessage ="Emai is required"),EmailAddress(ErrorMessage = "Invalid Email")]    
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]


        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirm Password doesnt match password")]
        public string ConfirmPassword { get; set; }

        public bool Agree { get; set; }

    }
}
