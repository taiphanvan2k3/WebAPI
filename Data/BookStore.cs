namespace LearnApiWeb.Data
{
    public class BookStore
    {
        public BookStore()
        {
            Books = new List<Book>();
        }

        public int MaCuaHang { get; set; }

        public string TenCuaHang { get; set; } = null!;

        public virtual List<Book> Books { get; set; }
    }
}