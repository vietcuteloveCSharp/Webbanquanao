using DTO.VuvietanhDTO.Sanphams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.ValidateCustomDTO
{
    public class NotBeforeCreationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance as CreatSanPhamDTO;
            if (value is DateTime ngayCapNhat && instance != null)
            {
                if (ngayCapNhat < instance.NgayTao)
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày cập nhật không thể trước ngày tạo.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
