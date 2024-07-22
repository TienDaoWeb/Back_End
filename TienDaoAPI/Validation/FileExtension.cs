using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var filename = Path.GetExtension(file.FileName);
                string[] exts = { "png", "jpg", "jpeg" };


                bool result = exts.Any(ext => filename.EndsWith(ext));
                if (!result)
                {
                    return new ValidationResult("Allowed extension file png or jpg");
                }

            }
            return ValidationResult.Success;
        }
    }
}
