using LearnApiWeb.Models;
using LearnApiWeb.Repositories;

namespace LearnApiWeb.Extensions
{
    public static class AppConfigure
    {
        public static void AddDependencyService(this IServiceCollection services)
        {
            // Tuy có nhiều class implement 1 interface nhưng khi đăng kí
            // thì chỉ có 1 class implement thôi
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}