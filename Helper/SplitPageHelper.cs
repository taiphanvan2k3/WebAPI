namespace LearnApiWeb.Models
{
    public class SplitPageHelper<T>
    {
        public static IQueryable<T> SplitPage(IQueryable<T> source, int pageIndex, int pageSize)
        {
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}