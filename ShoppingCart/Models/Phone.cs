using PhoneStore.Context.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStore.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Минимум 5 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введите цену продукта")]
        [Display(Name = "Цена продукта")]
        public double Price { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Выберите бренд продукта")]
        [Display(Name = "Категория продукта")]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Display(Name = "Изображение")]
        public string Image { get; set; } = "noimage.jpg";

        public int CountSales { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }

        [NotMapped]
        public string STerm { get; set; }

    }
}
