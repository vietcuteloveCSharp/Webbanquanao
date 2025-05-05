using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.HoadonsDTO
{
    public class UpdateTrangThaiDTO
    {
        public int Id { get; set; }
        public ETrangThaiHD TrangThai { get; set; }
        public string diaChiGiaoHang { get; set; }
        public int Id_NhanVien { get; set; }
   
    }
}
