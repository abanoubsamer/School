using System.ComponentModel.DataAnnotations;

namespace School.Models

{
    public class RegesStudrnt:ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var std = (Student)validationContext.ObjectInstance;

            if (std.StartDate == null)
            {
                return new ValidationResult("Enrollment Date is Required");
            }
            else
            {
                // Calculate difference in years from StartDate to today
                var date = DateTime.Today.Year - std.StartDate.Value.Year;

                // Validate based on the calculated 'date' if needed
                // For example, if you want to validate based on the years of enrollment
                if (date < 0 || date > 3) // Example validation logic
                {
                    return new ValidationResult("Enrollment must be within the last 3 years");
                }
            }

            return ValidationResult.Success; // Validation passed
        }

    }
}
