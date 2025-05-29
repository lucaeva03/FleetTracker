using System.ComponentModel.DataAnnotations;

namespace FleetTracker.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
		public string? username { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
		public string? password { get; set; }
	}
}