using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SewaPatra.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Number is required")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Number must be between 10 and 15 digits")]
        public string Number { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string Password { get; set; }

        ///[Required(ErrorMessage = "New Password is required")]
        //[DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string? NewPassword { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required")]
        //[DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string? ConfirmPassword { get; set; }


        // TO hash the password
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        //Verifying password
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return string.Equals(enteredPasswordHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
