using System.ComponentModel.DataAnnotations;

namespace web_db.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string Password { get; set; }
    }
}
