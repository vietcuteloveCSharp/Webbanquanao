using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Calamviecs
{
    public class CreateCaLamViecDTO
    {
        [Required(ErrorMessage = "TenCa is required.")]
        public EnumTenCa TenCa { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public bool TrangThai { get; set; } =true;
        [Required(ErrorMessage = "Id NgayLamViec is required.")]
        public int IdNgayLamViec { get; set; }
    }
}
