using System.ComponentModel.DataAnnotations;

namespace VitaCore.Models
{
    public class LoginModel
    {
        //[Display(Name = "E-Mail")]
        //[EmailAddress]
        //public string? Email { get; set; }

        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        //[NotMapped]
        //public string ReturnUrl { get; set; }
        //[NotMapped]
        //public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is mandatory!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        //compare below
        [Compare("Password", ErrorMessage = "The password is not the same")]
        public string PasswordConfirmation { get; set; }
    }

    public class LoginModelMaui
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class RegisterModelMaui
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string MauiPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfilePictureBase64 { get; set; }
    }

    public class UserProfileDto
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureBase64 { get; set; }
    }
}
