using Bogus;
using DAL.Entities;
using DTO.Hoangnn;
using Enum.EnumVVA;
using static DAL.DataSeed.EnumableClass;

namespace DAL.DataSeed
{
    public class DatabaseSeeder
    {
        public IReadOnlyCollection<ChiTietHoaDon> ChiTietHoaDons { get; } = new List<ChiTietHoaDon>();
        public IReadOnlyCollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; } = new List<ChiTietKhuyenMai>();
        public IReadOnlyCollection<ChiTietMaGiamGia> ChiTietMaGiamGias { get; } = new List<ChiTietMaGiamGia>();
        public IReadOnlyCollection<ChiTietSanPham> ChiTietSanPhams { get; } = new List<ChiTietSanPham>();
        public IReadOnlyCollection<ChucVu> ChucVus { get; } = new List<ChucVu>();
        public IReadOnlyCollection<CuaHang> CuaHangs { get; } = new List<CuaHang>();
        public IReadOnlyCollection<DanhMuc> DanhMucs { get; } = new List<DanhMuc>();
        public IReadOnlyCollection<GioHang> GioHangs { get; } = new List<GioHang>();
        public IReadOnlyCollection<HoaDon> HoaDons { get; } = new List<HoaDon>();
        public IReadOnlyCollection<KhachHang> KhachHangs { get; } = new List<KhachHang>();
        public IReadOnlyCollection<KhuyenMai> KhuyenMais { get; } = new List<KhuyenMai>();
        public IReadOnlyCollection<KichThuoc> KichThuocs { get; } = new List<KichThuoc>();
        public IReadOnlyCollection<MaGiamGia> MaGiamGias { get; } = new List<MaGiamGia>();
        public IReadOnlyCollection<MauSac> MauSacs { get; } = new List<MauSac>();
        public IReadOnlyCollection<NhanVien> NhanViens { get; } = new List<NhanVien>();
        public IReadOnlyCollection<PhuongThucThanhToan> PhuongThucThanhToans { get; } = new List<PhuongThucThanhToan>();
        public IReadOnlyCollection<SanPham> SanPhams { get; } = new List<SanPham>();
        public IReadOnlyCollection<ThanhToanHoaDon> ThanhToanHoaDons { get; } = new List<ThanhToanHoaDon>();
        public IReadOnlyCollection<ThuongHieu> ThuongHieus { get; } = new List<ThuongHieu>();
        public IReadOnlyCollection<HinhAnh> HinhAnhs { get; } = new List<HinhAnh>();

        public DatabaseSeeder()
        {
            CuaHangs = GenerateCuaHangs(1);
            KhuyenMais = GenerateKhuyenMais(20);
            DanhMucs = GenerateDanhMucs(20);
            ChiTietKhuyenMais = GenerateChiTietKhuyenMais(100, KhuyenMais, DanhMucs);
            MauSacs = GenerateMauSacs(10);
            KichThuocs = GenerateKichThuocs(10);
            ThuongHieus = GenerateThuongHieus(10);
            SanPhams = GenerateSanPhams(50, ThuongHieus, DanhMucs);
            ChiTietSanPhams = GenerateChiTietSanPhams(100, SanPhams, MauSacs, KichThuocs);
            KhachHangs = GenerateKhachHangs(50);
            GioHangs = GenerateGioHangs(100, KhachHangs, ChiTietSanPhams);
            PhuongThucThanhToans = GeneratePhuongThucThanhToans(10);
            ChucVus = GenerateChucVus(10);
            NhanViens = GenerateNhanViens(10, ChucVus);
            HoaDons = GenerateHoaDons(50, KhachHangs, NhanViens);
            ThanhToanHoaDons = GenerateThanhToanHoaDons(50, PhuongThucThanhToans, HoaDons);
            ChiTietHoaDons = GenerateChiTietHoaDons(100, ChiTietSanPhams, HoaDons);
            MaGiamGias = GenerateMaGiamGias(50);
            ChiTietMaGiamGias = GenerateChiTietMaGiamGias(100, MaGiamGias, KhachHangs);
            HinhAnhs = GenerateHinhAnhs(250, SanPhams);
        }

        private IReadOnlyCollection<HinhAnh> GenerateHinhAnhs(int amount, IEnumerable<SanPham> sanPhams)
        {
            var dieuKienId = 1;
            var lstFaker = new Faker<HinhAnh>(locale: "vi");



            lstFaker = lstFaker.RuleFor(x => x.Id_SanPham, f => f.PickRandom(sanPhams).Id);
            for (global::System.Int32 i = 0; i < 5; i++)
            {
                lstFaker = lstFaker
            .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
            .RuleFor(x => x.Url, f => f.Image.PicsumUrl(800, 1000));
            }
            var hinhAnhs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(lstFaker, i))
                .ToList();
            return hinhAnhs;
        }

        // Seed table CuHang
        private static IReadOnlyCollection<CuaHang> GenerateCuaHangs(int amount)
        {
            var cuaHangId = 1;

            var cuaHangFaker = new Faker<CuaHang>(locale: "vi")
                .RuleFor(x => x.Id, f => cuaHangId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Name.FullName())
                .RuleFor(x => x.DiaChi, f => f.Address.FullAddress())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Sdt, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(x => x.NgayTao, f => f.Date.FutureOffset(
                    refDate: new DateTimeOffset(2024, 1, 16, 15, 15, 0, TimeSpan.FromHours(1))).DateTime);

            var cuaHangs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(cuaHangFaker, i))
                .ToList();

            return cuaHangs;
        }

        //Seed table MaGiamGia
        private static IReadOnlyCollection<MaGiamGia> GenerateMaGiamGias(int amount)
        {
            var dieuKienId = 1;


            var giamGiaFaker = new Faker<MaGiamGia>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.LoaiGiamGia, f => (int)f.PickRandom<LoaiGiamGiaEnum>())
                .RuleFor(x => x.DieuKienGiamGia, f => f.Random.Int(200000, 1000000))
                .RuleFor(x => x.GiaTriGiam, (f, _) => f.Random.Decimal(10, 50))
                 .RuleFor(x => x.MenhGia, (f, _) => f.Random.Decimal(100000, 500000))
               .RuleFor(x => x.GiaTriToiDa, (f, _) => f.Random.Decimal(100000, 500000))
        .RuleFor(x => x.NoiDung, f => f.Lorem.Sentence(39))
        .RuleFor(x => x.TrangThai, f => (int)f.PickRandom<TrangThaiMaGiamGiaEnum>());

            var giamGias = Enumerable.Range(1, amount)
                .Select(i => SeedRow(giamGiaFaker, i))
                .ToList();

            return giamGias;
        }

        //Seed table KhuyenMai
        private static IReadOnlyCollection<KhuyenMai> GenerateKhuyenMais(int amount)
        {
            var dieuKienId = 1;

            var khuyenMaiFaker = new Faker<KhuyenMai>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Random.Words(10))
                .RuleFor(x => x.MoTa, f => f.Lorem.Sentence(39))
                .RuleFor(x => x.NgayTao, f => DateTime.Now.AddDays(-5))
                .RuleFor(x => x.NgayBatDau, f => f.Date.PastOffset(-4, DateTimeOffset.Now).DateTime)
                .RuleFor(x => x.NgayKetThuc, f => f.Date.SoonOffset(7, DateTimeOffset.Now).DateTime)
                .RuleFor(x => x.TrangThai, f => f.Random.Bool());

            var khuyenMais = Enumerable.Range(1, amount)
                .Select(i => SeedRow(khuyenMaiFaker, i))
                .ToList();

            return khuyenMais;
        }

        //Seed table DanhMuc
        private static IReadOnlyCollection<DanhMuc> GenerateDanhMucs(int amount)
        {
            var dieuKienId = 1;

            var danhMucFaker = new Faker<DanhMuc>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Id_DanhMucCha, f => f.Random.Number(1, amount))
                .RuleFor(x => x.TenDanhMuc, f => f.Commerce.ProductName())
                .RuleFor(x => x.NgayTao, f => f.Date.Recent())
                .RuleFor(x => x.TrangThai, f => f.Random.Bool());

            var danhMucs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(danhMucFaker, i))
                .ToList();

            return danhMucs;
        }

        //Seed table ChiTietKhuyenMai
        private static IReadOnlyCollection<ChiTietKhuyenMai> GenerateChiTietKhuyenMais(int amount, IEnumerable<KhuyenMai> khuyenmais, IEnumerable<DanhMuc> danhMucs)
        {
            var dieuKienId = 1;

            var chiTietKhuyenMaiFaker = new Faker<ChiTietKhuyenMai>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.LoaiKhuyenMai, f => f.Random.Number(0, 1))
                .RuleFor(x => x.GiaTriGiam, f => f.Commerce.Price(5, 80))
                 .RuleFor(x => x.MenhGia, (f, _) => f.Random.Decimal(100000, 500000))
                       .RuleFor(x => x.GiaTriToiDa, (f, _) => f.Random.Decimal(100000, 500000))
                .RuleFor(x => x.Id_KhuyenMai, f => f.PickRandom(khuyenmais).Id)
                .RuleFor(x => x.Id_DanhMuc, f => f.PickRandom(danhMucs).Id);

            var chiTietKhuyenMais = Enumerable.Range(1, amount)
                .Select(i => SeedRow(chiTietKhuyenMaiFaker, i))
                .ToList();

            return chiTietKhuyenMais;
        }

        //Seed table MauSac
        private static IReadOnlyCollection<MauSac> GenerateMauSacs(int amount)
        {
            var dieuKienId = 1;
            var listColor = ListColorInfo(amount);
            var mauSacFaker = new Faker<MauSac>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, (f, u) => listColor.ElementAt((u.Id - 1)).Name)
                .RuleFor(x => x.MaHex, (f, u) => listColor.ElementAt((u.Id - 1)).HexCode);

            var mauSacs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(mauSacFaker, i))
                .ToList();

            return mauSacs;
        }

        //Seed table KichThuoc
        private static IReadOnlyCollection<KichThuoc> GenerateKichThuocs(int amount)
        {
            var dieuKienId = 1;
            var kichThuocSize = new[] { "S", "M", "L", "XL", "XXL" };
            var kichThuocFaker = new Faker<KichThuoc>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.PickRandom(kichThuocSize));

            var kichThuocs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(kichThuocFaker, i))
                .ToList();

            return kichThuocs;
        }

        //Seed table ThuongHieu
        private static IReadOnlyCollection<ThuongHieu> GenerateThuongHieus(int amount)
        {
            var dieuKienId = 1;

            var thuongHieuFaker = new Faker<ThuongHieu>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Company.CompanySuffix())
                .RuleFor(x => x.MoTa, f => f.Company.CompanyName())
                .RuleFor(x => x.TrangThai, f => f.Random.Bool());

            var thuongHieus = Enumerable.Range(1, amount)
                .Select(i => SeedRow(thuongHieuFaker, i))
                .ToList();

            return thuongHieus;
        }

        //Seed table SanPham
        private static IReadOnlyCollection<SanPham> GenerateSanPhams(int amount, IEnumerable<ThuongHieu> thuongHieus, IEnumerable<DanhMuc> danhMucs)
        {
            var dieuKienId = 1;

            var sanPhamFaker = new Faker<SanPham>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Commerce.ProductName())
                .RuleFor(x => x.MoTa, f => f.Lorem.Word())
               .RuleFor(x => x.Gia, f => Convert.ToDecimal(f.Commerce.Price(50000, 10000000)))
                .RuleFor(x => x.SoLuong, f => f.Random.Number(100, 100000))
                .RuleFor(x => x.NgayTao, f => f.Date.PastOffset(30).DateTime)
                .RuleFor(x => x.NgayCapNhat, f => f.Date.PastOffset(5).DateTime)
                .RuleFor(x => x.TrangThai, f => f.Random.Bool())
                .RuleFor(x => x.HinhAnh, f => f.Image.PicsumUrl(800, 1000))
                .RuleFor(x => x.Id_ThuongHieu, f => f.PickRandom<ThuongHieu>(thuongHieus).Id)
                .RuleFor(x => x.Id_DanhMuc, f => f.PickRandom<DanhMuc>(danhMucs).Id);

            var sanPhams = Enumerable.Range(1, amount)
                .Select(i => SeedRow(sanPhamFaker, i))
                .ToList();

            return (IReadOnlyCollection<SanPham>)sanPhams.AsReadOnly();
        }

        //Seed table ChiTietSanPham
        private static IReadOnlyCollection<ChiTietSanPham> GenerateChiTietSanPhams(int amount, IEnumerable<SanPham> sanPhams, IEnumerable<MauSac> mauSacs, IEnumerable<KichThuoc> kichThuocs)
        {
            var dieuKienId = 1;

            var chiTietSanPhamFaker = new Faker<ChiTietSanPham>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.SoLuong, f => f.Random.Number(100, 100000))
                .RuleFor(x => x.NgayTao, f => f.Date.PastOffset(30).DateTime)
                .RuleFor(x => x.TrangThai, f => f.Random.Bool())
                .RuleFor(x => x.Id_SanPham, f => f.PickRandom<SanPham>(sanPhams).Id)
                .RuleFor(x => x.Id_KichThuoc, f => f.PickRandom<KichThuoc>(kichThuocs).Id)
                .RuleFor(x => x.Id_MauSac, f => f.PickRandom<MauSac>(mauSacs).Id);


            var chiTietSanPhams = Enumerable.Range(1, amount)
                .Select(i => SeedRow(chiTietSanPhamFaker, i))
                .ToList();

            return chiTietSanPhams;
        }

        //Seed table KhachHang
        private static IReadOnlyCollection<KhachHang> GenerateKhachHangs(int amount)
        {
            var dieuKienId = 1;

            var khachHangFaker = new Faker<KhachHang>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Name.FullName())
                .RuleFor(x => x.TaiKhoan, (f, u) => f.Internet.UserName(u.Ten))
                .RuleFor(x => x.MatKhau, f => f.Internet.Password())
                .RuleFor(x => x.NgaySinh, f => f.Date.BetweenOffset(DateTimeOffset.Now.AddYears(-60), DateTimeOffset.Now).DateTime)
                .RuleFor(x => x.Sdt, f => f.Phone.PhoneNumber("09#######"))
                .RuleFor(x => x.Email, (f, u) => f.Internet.Email(f.Name.FirstName(), f.Name.LastName(), u.NgaySinh.Value.Year.ToString()))
                .RuleFor(x => x.Avatar, f => f.Internet.Avatar())
                .RuleFor(x => x.TrangThai, f => f.Random.Bool());

            var khachHangs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(khachHangFaker, i))
                .ToList();

            return khachHangs;
        }

        //Seed table GioHang
        private static IReadOnlyCollection<GioHang> GenerateGioHangs(int amount, IEnumerable<KhachHang> khachHangs, IEnumerable<ChiTietSanPham> chiTietSanPhams)
        {
            var dieuKienId = 1;

            var gioHangFaker = new Faker<GioHang>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.SoLuong, f => f.Random.Number(1, 100))
                .RuleFor(x => x.TrangThai, f => f.Random.Bool())
                .RuleFor(x => x.Id_KhachHang, f => f.PickRandom<KhachHang>(khachHangs).Id)
                .RuleFor(x => x.Id_ChiTietSanPham, f => f.PickRandom<ChiTietSanPham>(chiTietSanPhams).Id);


            var gioHangs = Enumerable.Range(1, amount)
                .Select(i => SeedRow(gioHangFaker, i))
                .ToList();

            return gioHangs;
        }

        //Seed table PhuongThucThanhToan
        private static IReadOnlyCollection<PhuongThucThanhToan> GeneratePhuongThucThanhToans(int amount)
        {
            var dieuKienId = 1;

            var phuongThucThanhToanFaker = new Faker<PhuongThucThanhToan>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Finance.AccountName())
                .RuleFor(x => x.Mota, f => f.Finance.TransactionType())
                .RuleFor(x => x.NgayTao, f => f.Date.Recent())
                .RuleFor(x => x.TrangThai, f => f.Random.Bool());

            var phuongThucThanhToans = Enumerable.Range(1, amount)
                .Select(i => SeedRow(phuongThucThanhToanFaker, i))
                .ToList();

            return phuongThucThanhToans;
        }

        //Seed table ChucVu
        private static IReadOnlyCollection<ChucVu> GenerateChucVus(int amount)
        {
            var dieuKienId = 1;

            var chucVuFaker = new Faker<ChucVu>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.Ten, f => f.Commerce.Department())
                .RuleFor(x => x.Mota, f => f.Lorem.Word());

            var chucVus = Enumerable.Range(1, amount)
                .Select(i => SeedRow(chucVuFaker, i))
                .ToList();

            return chucVus;
        }

        //Seed table NhanVien
        private static IReadOnlyCollection<NhanVien> GenerateNhanViens(int amount, IEnumerable<ChucVu> chucVus)
        {
            var dieuKienId = 1;

            var nhanVienFaker = new Faker<NhanVien>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.TenNhanVien, f => f.Name.FullName())
                .RuleFor(x => x.TaiKhoan, (f, u) => f.Internet.UserName(u.TenNhanVien))
                .RuleFor(x => x.MatKhau, (f, u) => f.Internet.Password(15))
                .RuleFor(x => x.Sdt, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(x => x.NgaySinh, f => f.Date.BetweenOffset(DateTimeOffset.Now.AddYears(-40), DateTimeOffset.Now).DateTime)
                .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.TenNhanVien))
                .RuleFor(x => x.DiaChi, f => f.Address.FullAddress())
                .RuleFor(x => x.GhiChu, f => f.Lorem.Sentence(30))
                .RuleFor(x => x.TrangThai, f => f.Random.Bool())
                .RuleFor(x => x.NgayTao, f => f.Date.BetweenOffset(DateTimeOffset.Now.AddYears(-1), DateTimeOffset.Now.AddDays(-7)).DateTime)
                .RuleFor(x => x.NgayCapNhat, f => f.Date.Recent())
                .RuleFor(x => x.Id_ChucVu, f => f.PickRandom<ChucVu>(chucVus).Id);


            var nhanViens = Enumerable.Range(1, amount)
                .Select(i => SeedRow(nhanVienFaker, i))
                .ToList();

            return nhanViens;
        }

        //Seed table HoaDon
        private static IReadOnlyCollection<HoaDon> GenerateHoaDons(int amount, IEnumerable<KhachHang> khachHangs, IEnumerable<NhanVien> nhanViens)
        {
            var dieuKienId = 1;

            var hoaDonFaker = new Faker<HoaDon>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.TongTien, f => decimal.Parse(f.Commerce.Price(200000, 10000000)))
                .RuleFor(x => x.NgayTao, f => f.Date.Recent())
                .RuleFor(x => x.TrangThai, f => f.PickRandom<ETrangThaiHD>())
                .RuleFor(x => x.Id_NhanVien, f => f.PickRandom(nhanViens).Id)
                .RuleFor(x => x.Id_KhachHang, f => f.PickRandom(khachHangs).Id);


            var hoaDons = Enumerable.Range(1, amount)
                .Select(i => SeedRow(hoaDonFaker, i))
                .ToList();

            return hoaDons;
        }

        //Seed table ThanhToanHoaDon
        private static IReadOnlyCollection<ThanhToanHoaDon> GenerateThanhToanHoaDons(int amount, IEnumerable<PhuongThucThanhToan> phuongThucThanhToans, IEnumerable<HoaDon> hoaDons)
        {
            var dieuKienId = 1;

            var thanhToanHoaDonFaker = new Faker<ThanhToanHoaDon>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.TongTien, f => decimal.Parse(f.Commerce.Price(200000, 10000000)))
                .RuleFor(x => x.SoTienDaThanhToan, f => decimal.Parse(f.Commerce.Price(200000, 10000000)))
                .RuleFor(x => x.NgayThanhToan, f => f.Date.Recent())
                .RuleFor(x => x.MaGiaoDich, f => f.Random.Guid().ToString())
                .RuleFor(x => x.Id_HoaDon, f => f.PickRandom(hoaDons).Id)
                .RuleFor(x => x.Id_PhuongThucThanhToan, f => f.PickRandom(phuongThucThanhToans).Id);


            var thanhToanHoaDons = Enumerable.Range(1, amount)
                .Select(i => SeedRow(thanhToanHoaDonFaker, i))
                .ToList();

            return thanhToanHoaDons;
        }

        //Seed table ChiTietHoaDon
        private static IReadOnlyCollection<ChiTietHoaDon> GenerateChiTietHoaDons(int amount, IEnumerable<ChiTietSanPham> chiTietSanPhams, IEnumerable<HoaDon> hoaDons)
        {
            var dieuKienId = 1;

            var chiTietHoaDonFaker = new Faker<ChiTietHoaDon>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.SoLuong, f => f.Random.Number(1, 50))
                .RuleFor(x => x.Gia, f => decimal.Parse(f.Commerce.Price(200000, 10000000)))
                .RuleFor(x => x.TrangThai, f => f.Random.Bool())
                .RuleFor(x => x.Id_HoaDon, f => f.PickRandom(hoaDons).Id)
                .RuleFor(x => x.Id_ChiTietSanPham, f => f.PickRandom(chiTietSanPhams).Id);


            var chiTietHoaDons = Enumerable.Range(1, amount)
                .Select(i => SeedRow(chiTietHoaDonFaker, i))
                .ToList();

            return chiTietHoaDons;
        }

        //Seed table ChiTietMaGiamGia
        private static IReadOnlyCollection<ChiTietMaGiamGia> GenerateChiTietMaGiamGias(int amount, IEnumerable<MaGiamGia> maGiamGias, IEnumerable<KhachHang> khachHangs)
        {
            var dieuKienId = 1;

            var chiTietMaGiamGiaFaker = new Faker<ChiTietMaGiamGia>(locale: "vi")
                .RuleFor(x => x.Id, f => dieuKienId++) // Each product will have an incrementing id.
                .RuleFor(x => x.NoiDung, f => f.Lorem.Sentence(30))
                .RuleFor(x => x.NgaySuDung, f => f.Date.Recent())
                .RuleFor(x => x.Id_MaGiamGia, f => f.PickRandom(maGiamGias).Id)
                .RuleFor(x => x.Id_KhachHang, f => f.PickRandom(khachHangs).Id);


            var chiTietMaGiamGias = Enumerable.Range(1, amount)
                .Select(i => SeedRow(chiTietMaGiamGiaFaker, i))
                .ToList();

            return chiTietMaGiamGias;
        }

        private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class
        {
            var recordRow = faker.UseSeed(rowId).Generate();
            return recordRow;
        }

        //method ListColorInfo()

        private static List<ColorInfo> ListColorInfo(int amout)
        {
            var faker = new Faker();
            var colors = new List<ColorInfo>();
            for (int i = 0; i < amout; i++)
            {
                var randomColor = faker.Commerce.Color(); // Tạo màu ngẫu nhiên
                var hexColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromName(randomColor));
                colors.Add(new ColorInfo { Name = randomColor, HexCode = hexColor });
            }
            return colors;
        }

    }
}
