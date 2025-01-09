﻿using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.HoadonsDTO
{
    public class HoaDonDTO
    {
       
        public string TongTien { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public decimal PhiVanChuyen { get; set; } // tiền phí vận chuyển
        public ETrangThaiHD TrangThai { get; set; }= ETrangThaiHD.ChoXuLy;
        public int Id_NhanVien { get; set; }
    
        public int Id_KhachHang { get; set; }
    }
}
