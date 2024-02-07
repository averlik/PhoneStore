using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MinLength(2)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
