namespace LearnApiWeb.Data
{
    public enum TinhTrangDonDatHang
    {
        New = 0,
        Payment = 1,
        Complete = 2,
        Cancel = -1
    }

    public class DonHang
    {
        public DonHang()
        {
            // Để đảm bảo rằng không muốn DonHangChiTiets là null vì có thể khi Linq
            // ta dùng đến nó nhưng vô tình bị null dẫn đến lỗi
            // => Ta cần tạo cho nó 1 List rỗng
            DonHangChiTiets = new List<DonHangChiTiet>();
        }

        public Guid MaDonHang { get; set; }

        public DateTime NgayDatHang { get; set; }

        public DateTime? NgayGiao { get; set; }

        public TinhTrangDonDatHang TinhTrangDonHang { get; set; }

        public string TenNguoiNhan { get; set; } = null!;

        public string DiaChiGiao { get; set; } = null!;

        public string SoDienThoai { get; set; } = null!;

        public virtual List<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}