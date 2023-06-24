using LearnApiWeb.Models;

namespace LearnApiWeb.Repositoriesspace
{
    public interface IProductRepository
    {
        public Task<List<ProductViewModel>> FilterProducts(string pattern);
    }
}