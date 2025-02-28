﻿using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Calamviecs
{
    public class CaLamViecDTO
    {

        public EnumTenCa TenCa { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public bool TrangThai { get; set; }
        public int Id_NhanVien { get; set; }
    }
}
