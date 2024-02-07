using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, MinLength(2, ErrorMessage = "Минимум 2 символа")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Минимум 4 символа")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
