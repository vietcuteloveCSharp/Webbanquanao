using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NTTuyenDTO.BaseShifts
{
    internal class FullBaseShiftDTO
    {

       
        public int Id { get; set; }
       
        public string TenCa { get; set; }
        
        public TimeSpan GioBatDau { get; set; }
       
        public TimeSpan GioKetThuc { get; set; }
        public bool TrangThai { get; set; } = true;
    }
}
