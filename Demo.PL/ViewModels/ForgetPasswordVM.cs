using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ForgetPasswordVM
	{

		[Required(ErrorMessage ="Email is Required")]
		public string Email { get; set; }
	}
}
