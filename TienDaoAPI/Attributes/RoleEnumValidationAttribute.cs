using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Enums;

namespace TienDaoAPI.Attributes
{
    public class RoleEnumValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is string role))
            {
                return new ValidationResult("Role is required.");
            }

            if (role != RoleEnum.ADMIN && role != RoleEnum.CONVERTER && role != RoleEnum.READER)
            {
                return new ValidationResult($"Invalid role: {role}. Allowed values are: Admin, Converter, Reader.");
            }

            return ValidationResult.Success!;
        }
    }
}
