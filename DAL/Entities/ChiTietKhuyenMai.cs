using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChiTietKhuyenMai
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [ForeignKey("KhuyenMai")]
        public int? Id_KhuyenMai { get; set; } = null;
        [ForeignKey("DanhMuc")]
        public int? Id_DanhMuc { get; set; } = null;

        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
    }
}
