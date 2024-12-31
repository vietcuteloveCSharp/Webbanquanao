using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class HinhAnh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? ImageData { get; set; }
        [ForeignKey("SanPham")]
        public int Id_SanPham { get; set; }
        
        public int ImageSourceType { get; set; } // 0: File, 1: Link URL
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public  SanPham SanPham { get; set; }

        [NotMapped]
        [FileExtensions]

        public IFormFile? ImageUpload { get; set; }
    }
}
