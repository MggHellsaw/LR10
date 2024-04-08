using System;
using System.ComponentModel.DataAnnotations;

namespace LR10.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Поле Ім'я прізвище є обов'язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Бажана дата консультації є обов'язковим")]
        [FutureDate(ErrorMessage = "Дата консультації має бути в майбутньому")]
        [Weekday(ErrorMessage = "Дата консультації не може бути вихідним днем")]
        [ProductAndDay(ErrorMessage = "Консультація щодо продукту «Основи» не може проходити по понеділках.")]
        public DateTime ConsultationDate { get; set; }

        [Required(ErrorMessage = "Поле Продукт є обов'язковим")]
        public string SelectedProduct { get; set; }

        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime date = Convert.ToDateTime(value);
                return date.Date > DateTime.Today && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            }
        }

        public class WeekdayAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime date = Convert.ToDateTime(value);
                return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            }
        }

        public class ProductAndDayAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = (RegistrationModel)validationContext.ObjectInstance;

                if (model.SelectedProduct == "Основи" && model.ConsultationDate.DayOfWeek == DayOfWeek.Monday)
                {
                    return new ValidationResult("Консультація щодо продукту «Основи» не може проходити по понеділках.");
                }

                return ValidationResult.Success;
            }
        }
    }
}