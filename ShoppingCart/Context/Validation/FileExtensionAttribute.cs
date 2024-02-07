using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Context.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                string[] allowedExtensions = { ".jpg", ".png" }; // Разрешенные расширения

                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return "Допускаются только файлы форматов .jpg и .png";
        }
    }
}
