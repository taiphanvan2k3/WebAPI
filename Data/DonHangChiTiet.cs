namespace LearnApiWeb.Data
{
    public class DonHangChiTiet
    {
        public Guid MaDonHang { get; set; }

        public Guid MaHangHoa { get; set; }

        public int SoLuongMua { get; set; }


        // Đơn giá này là đơn giá tại thời điểm khách mua hàng
        // Nó sẽ khác với đơn giá hiện tại của sản phẩm tại thời điểm hiện tại vì có thể đã tăng/giảm giá
        public double DonGia { get; set; }


        // Phần trăm giảm giá lúc mua
        public double GiamGia { get; set; }

        // Relationshop
        public virtual DonHang DonHang { get; set; }

        public virtual Product Product { get; set; }
    }
}