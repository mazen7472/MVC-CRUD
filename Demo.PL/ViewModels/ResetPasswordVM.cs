using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordVM
	{
		[Required(ErrorMessage = "Password is required")]


		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Confirm Password doesnt match password")]
		public string ConfirmPassword { get; set; }
	}
}
