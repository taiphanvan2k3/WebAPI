using LearnApiWeb.Data;
using LearnApiWeb.Dtos;
using LearnApiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bookv2Controller : ControllerBase
    {
        private readonly IBookRepository _bookRepos;

        public Bookv2Controller(IBookRepository bookRepos)
        {
            _bookRepos = bookRepos;
        }

        [HttpGet]
        // public async Task<IActionResult> GetAllBooks()
        // public async Task<ActionResult> GetAllBooks()
        public async Task<ActionResult<List<BookModel>>> GetAllBooks()
        {
            try
            {
                return Ok(await _bookRepos.GetAllBooksAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("book-min")]
        public async Task<IActionResult> GetAllBooksMin()
        {
            try
            {
                return Ok(await _bookRepos.GetAllBooksMinAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookModel>> GetBookById(int id)
        {
            // Có thể return IActionResult cũng được
            var book = await _bookRepos.GetBookAsync(id);
            return book == null ? NotFound("Sách này không tìm thấy") : book;
        }

        [HttpPost]
        public async Task<ActionResult<BookModel>> AddNewBook(BookModel book)
        {
            try
            {
                // Cần lấy ra id sách vừa được thêm để trả về thông tin của sách vừa được thêm ở request body
                int newId = await _bookRepos.AddBookAsync(book);
                var newBook = await _bookRepos.GetBookAsync(newId);
                // Nếu thêm mới thành công thì gọi phương thức CreatedAtAction
                return CreatedAtAction("GetBookById", new { id = book.Id }, newBook);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookInfo(int id, BookModel book)
        {
            // Với id thì tự động nó hiểu là FromRoute
            // còn với book thì nó tự động hiểu là FromBody
            // => Nên không cần điền
            if (id != book.Id)
            {
                return NotFound();
            }

            await _bookRepos.UpdateBookAsync(id, book);
            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deletedBook = await _bookRepos.DeleteAsync(id);
            if (deletedBook == null)
            {
                return NotFound("Không tìm thấy sách bạn muốn xóa");
            }
            return Ok(deletedBook);
        }
    }
}