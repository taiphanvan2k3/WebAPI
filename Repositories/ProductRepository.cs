using LearnApiWeb.Data;
using LearnApiWeb.Models;
using LearnApiWeb.Repositoriesspace;

namespace LearnApiWeb.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BookStoreContext _context;

        public ProductRepository(BookStoreContext context)
        {
            _context = context;
        }
        public Task<List<ProductViewModel>> FilterProducts(string pattern)
        {
            return null;
        }
    }
}