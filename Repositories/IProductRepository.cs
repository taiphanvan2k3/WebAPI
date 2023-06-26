using LearnApiWeb.Models;

namespace LearnApiWeb.Repositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> FilterProducts(string searchName);
    }
}