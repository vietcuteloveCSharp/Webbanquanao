using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    
    public class CaLamViec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "TenCa is required.")]
        public EnumTenCa TenCa { get; set; }
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Giobatdau is required.")]
        public TimeSpan GioBatDau { get; set; }
        [Required(ErrorMessage = "Gioketthuc is required.")]
        [DataType(DataType.Time)]
        public TimeSpan GioKetThuc { get; set; }
        public bool TrangThai { get; set; } = true;
        [ForeignKey("NgayLamViec")]
        public int IdNgaylamviec { get; set; }
        public virtual NgayLamViec NgayLamViec { get; set; }
        public virtual ICollection<CaNhanVien> Canhanviens { get; set; }
    }
}
