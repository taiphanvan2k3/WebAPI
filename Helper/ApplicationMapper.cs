using AutoMapper;
using LearnApiWeb.Data;
using LearnApiWeb.Dtos;
using LearnApiWeb.Models;

namespace LearnApiWeb.Helper
{
    public class ApplicationMapper : Profile
    {
        // Ta sẽ phải khai báo các hàm map trong constructor của mapper
        public ApplicationMapper()
        {
            // Thêm ReverseMap để có thể map cả 2 chiều
            // Nếu không có thì chỉ có map từ Book sang BookModel
            CreateMap<Book, BookModel>().ReverseMap();

            // Vẫn có thể wrapper từ thuộc tính giữa entity và 1 model có ít thuộc tính hơn
            CreateMap<Book, BookMinModel>().ReverseMap();

            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Category, CategoryModelMin>().ReverseMap();
        }
    }
}