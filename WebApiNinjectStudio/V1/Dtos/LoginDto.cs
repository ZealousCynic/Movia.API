using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ReturnTokenDto
    {
        public string Token { get; set; }
    }

    public class ReturnUserIdDto
    {
        public string UserId { get; set; }
    }
}
