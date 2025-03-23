namespace DAL.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }  // Tên file ảnh
        public byte[] ImageData { get; set; } // Dữ liệu ảnh dưới dạng byte[]
        public string ContentType { get; set; } // Loại ảnh (image/png, image/jpeg, ...)
    }

}
