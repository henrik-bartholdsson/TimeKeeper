using System;
using System.ComponentModel.DataAnnotations;

namespace TimeKeeper.Ui.Validators
{
    public class DeviationValidator : ValidationAttribute
    {
        private string _startDate;

        public DeviationValidator(string startDate)
        {
            _startDate = startDate;

        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var propertyInfo = validationContext.ObjectType.GetProperty(_startDate);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            var startTime = DateTime.Parse("1/1/1970" + " " + propertyValue);
            var endTime = DateTime.Parse("1/1/1970" + " " + value);


            if (endTime > startTime)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Stop time must be grater then end time.");
            }
        }
    }
}
