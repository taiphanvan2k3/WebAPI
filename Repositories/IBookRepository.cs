using LearnApiWeb.Data;
using LearnApiWeb.Dtos;

namespace LearnApiWeb.Repositories
{
    public interface IBookRepository
    {
        public Task<List<BookModel>> GetAllBooksAsync();

        public Task<List<BookMinModel>> GetAllBooksMinAsync();

        public Task<BookModel> GetBookAsync(int id);

        public Task<int> AddBookAsync(BookModel book);

        public Task UpdateBookAsync(int id, BookModel book);

        public Task<Book> DeleteAsync(int id);

        public Task<int> GetIdOfLastestBook();
    }
}