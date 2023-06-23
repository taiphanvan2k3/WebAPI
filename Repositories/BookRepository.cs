using AutoMapper;
using LearnApiWeb.Data;
using LearnApiWeb.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetBookAsync(int id)
        {
            Book? book = await _context.Books.FindAsync(id);
            return _mapper.Map<BookModel>(book);
        }

        public async Task<int> AddBookAsync(BookModel book)
        {
            Book newBook = _mapper.Map<Book>(book);
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task UpdateBookAsync(int id, BookModel book)
        {
            if (id == book.Id)
            {
                var updateBook = _mapper.Map<Book>(book);
                _context.Books.Update(updateBook);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> DeleteAsync(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return book;
        }

        public async Task<List<BookMinModel>> GetAllBooksMinAsync()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookMinModel>>(books); 
        }
    }
}