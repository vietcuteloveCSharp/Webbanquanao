using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChiTietKhuyenMai
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LoaiKhuyenMai { get; set; } // 0 - theo % || 1 - theo Giá
        public string GiaTriGiam { get; set; } // % giảm
        public string MenhGia { get; set; } // số tiền giảm
        public string GiaTriToiDa { get; set; }  // số tiền tối đa có thể giảm

        [ForeignKey("KhuyenMai")]
        public int? Id_KhuyenMai { get; set; } = null;
        [ForeignKey("DanhMuc")]
        public int? Id_DanhMuc { get; set; } = null;

        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
    }
}
