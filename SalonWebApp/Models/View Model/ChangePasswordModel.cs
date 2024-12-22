using System.ComponentModel.DataAnnotations;

namespace SalonWebApp.Models
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(string oldPassword, string newPassword, string confirmNewPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
            ConfirmNewPassword = confirmNewPassword;
        }

        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password does not match.")]
        public string ConfirmNewPassword { get; set; }

    }
}
