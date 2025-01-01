﻿using DAL.Entities;

namespace WebView.NghiaDTO
{
    public class KhuyenMaiDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public bool TrangThai { get; set; } // false - ngừng khuyến mại || true - đang khuyến mại

        public virtual ICollection<ChiTietKhuyenMaiDTO> ChiTietKhuyenMaiDTOs { get; set; }
    }
}
