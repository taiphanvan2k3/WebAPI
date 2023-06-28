using System.Net;
using System.Net.Http.Headers;
using LearnApiWeb.Data;
using LearnApiWeb.Models;
using LearnApiWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepos;
        public CategoryController(ICategoryRepository categoryRepos)
        {
            _categoryRepos = categoryRepos;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryModel>>> GetAllCategories()
        {
            return Ok(await _categoryRepos.GetAllCategories());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepos.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Không tìm thấy danh mục này.");
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewCategory(CategoryModelMin category)
        {
            // Do API này có attribute là [Authorize] nên chỉ có đăng nhập
            // thành công thì mới dùng API này được vì nó yêu cầu 1 token mà token này chỉ được
            // generate sau khi login thành công
            int newId = await _categoryRepos.AddNewCategory(category);
            if (newId != -1)
            {
                CategoryModel newCategory = await _categoryRepos.GetCategoryById(newId);
                return CreatedAtAction("GetCategoryById", new { id = newId }, newCategory);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryModelMin model)
        {
            var category = await _categoryRepos.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Không tìm thấy danh mục mà bạn muốn cập nhật.");
            }
            await _categoryRepos.UpdateCategory(id, model);
            return Ok("Update thành công");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryModel>> DeleteCategory(int id)
        {
            try
            {
                CategoryModel deletedCategory = await _categoryRepos.DeleteCategory(id);
                if (deletedCategory == null)
                {
                    return NotFound("Không tìm thấy danh mục bạn muốn xóa");
                }
                return Ok(deletedCategory);
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}