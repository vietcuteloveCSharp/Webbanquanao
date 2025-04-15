using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
  
    public class CaNhanVien
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NhanVien")]
        public int IdNhanVien { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        [ForeignKey("CaLamViec")]
        public int IdCaLamViec { get; set; }
        public virtual CaLamViec CaLamViec { get; set; }
    }
}
