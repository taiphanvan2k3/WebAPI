using AutoMapper;
using LearnApiWeb.Data;
using LearnApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryModel>> GetAllCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                return _mapper.Map<CategoryModel>(category);
            }
            return null;
        }

        public async Task<int> AddNewCategory(CategoryModelMin category)
        {
            try
            {
                Category newCategory = _mapper.Map<Category>(category);
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
                return newCategory.Id;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return -1;
            }
        }

        public async Task UpdateCategory(int id, CategoryModelMin category)
        {
            // Category updatedCategory = _mapper.Map<Category>(category);
            // updatedCategory.Id = id;
            // _context.Categories.Update(updatedCategory);
            Category updatedCategory = _context.Categories.Find(id);
            if (updatedCategory != null)
            {
                updatedCategory.Name = category.Name;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CategoryModel> DeleteCategory(int id)
        {
            Category deletedCategory = await _context.Categories.FindAsync(id);
            if (deletedCategory != null)
            {
                _context.Categories.Remove(deletedCategory);
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryModel>(deletedCategory);
            }
            return null;
        }
    }
}