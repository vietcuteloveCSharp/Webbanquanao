using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CaLamViec_NgayLamViec_NhanVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("NhanVien")]    
        public int Id_NhanVien { get; set; }
        
        [ForeignKey("NgayLamViec")]
        public int Id_NgayLamViec { get; set; }
        
        [ForeignKey("CaLamViec")]
        public int Id_CaLamViec { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual NgayLamViec NgayLamViec { get; set; }
        public virtual CaLamViec CaLamViec { get; set; }
    }
}
