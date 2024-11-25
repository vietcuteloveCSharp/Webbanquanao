using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.ValidateCustomDTO
{
    public  class NotInFutureAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Kiểm tra nếu value là kiểu DateTime
            if (value is DateTime dateTime)
            {
                // So sánh với thời gian hiện tại
                if (dateTime > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày không được ở tương lai.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
