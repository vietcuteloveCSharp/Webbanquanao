using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class WebBanQuanAoDbContext : DbContext
    {
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public virtual DbSet<ChiTietMaGiamGia> ChiTietMaGiamGias { get; set; }
        public virtual DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public virtual DbSet<CaNhanVien> CaNhanViens { get; set; }
        public virtual DbSet<DanhGia> DanhGias { get; set; }
        public virtual DbSet <Image> Images { get; set; }
        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<CuaHang> CuaHangs { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<KichThuoc> KichThuocs { get; set; }
        public virtual DbSet<MaGiamGia> MaGiamGias { get; set; }
        public virtual DbSet<MauSac> MauSacs { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<NgayLamViec> NgayLamviecs { get; set; }
        public virtual DbSet<CaNhanVien> Canhanviens { get; set; }
        public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<ThanhToanHoaDon> ThanhToanHoaDons { get; set; }
        public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }
        public virtual DbSet<HinhAnh> HinhAnhs { get; set; }
        public virtual DbSet<CaLamViec> CaLamViecs { get; set; }
       

       

        public WebBanQuanAoDbContext()
        {

        }

        public WebBanQuanAoDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Generate seed data with Bogus
            //var databaseSeeder = new DatabaseSeeder();


            //// Apply the seed data on the tables
            //modelBuilder.Entity<CuaHang>().HasData(databaseSeeder.CuaHangs);
            //modelBuilder.Entity<KhuyenMai>().HasData(databaseSeeder.KhuyenMais);
            //modelBuilder.Entity<DanhMuc>().HasData(databaseSeeder.DanhMucs);
            //modelBuilder.Entity<ChiTietKhuyenMai>().HasData(databaseSeeder.ChiTietKhuyenMais);
            //modelBuilder.Entity<MauSac>().HasData(databaseSeeder.MauSacs);
            //modelBuilder.Entity<KichThuoc>().HasData(databaseSeeder.KichThuocs);
            //modelBuilder.Entity<ThuongHieu>().HasData(databaseSeeder.ThuongHieus);
            //modelBuilder.Entity<SanPham>().HasData(databaseSeeder.SanPhams);
            //modelBuilder.Entity<ChiTietSanPham>().HasData(databaseSeeder.ChiTietSanPhams);
            //modelBuilder.Entity<KhachHang>().HasData(databaseSeeder.KhachHangs);
            //modelBuilder.Entity<GioHang>().HasData(databaseSeeder.GioHangs);
            //modelBuilder.Entity<PhuongThucThanhToan>().HasData(databaseSeeder.PhuongThucThanhToans);
            //modelBuilder.Entity<ChucVu>().HasData(databaseSeeder.ChucVus);
            //modelBuilder.Entity<NhanVien>().HasData(databaseSeeder.NhanViens);
            //modelBuilder.Entity<HoaDon>().HasData(databaseSeeder.HoaDons);
            //modelBuilder.Entity<ThanhToanHoaDon>().HasData(databaseSeeder.ThanhToanHoaDons);
            //modelBuilder.Entity<ChiTietHoaDon>().HasData(databaseSeeder.ChiTietHoaDons);
            //modelBuilder.Entity<MaGiamGia>().HasData(databaseSeeder.MaGiamGias);
            //modelBuilder.Entity<ChiTietMaGiamGia>().HasData(databaseSeeder.ChiTietMaGiamGias);
            //modelBuilder.Entity<HinhAnh>().HasData(databaseSeeder.HinhAnhs);



            //base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=TUYEN_DEV\\SQLEXPRESS;Initial Catalog=QuanAoCanMan1;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");


        }
    }
}
