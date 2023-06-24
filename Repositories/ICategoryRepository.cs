using LearnApiWeb.Data;
using LearnApiWeb.Models;

namespace LearnApiWeb.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategories();

        public Task<CategoryModel> GetCategoryById(int id);

        public Task<int> AddNewCategory(CategoryModelMin category);

        public Task UpdateCategory(int id, CategoryModelMin category);
        
        public Task<CategoryModel> DeleteCategory(int id);
    }
}