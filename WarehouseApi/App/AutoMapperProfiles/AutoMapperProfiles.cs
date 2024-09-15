using AutoMapper;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Models.Category;
using WarehouseApi.Models.Order;
using WarehouseApi.Models.Product;

namespace WarehouseApi.App.AutoMapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequest>().ReverseMap();
            CreateMap<Category, UpdateCategoryStockRequest>().ReverseMap();
            CreateMap<Category, DeleteCategoryRequest>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductRequest>().ReverseMap();
            CreateMap<Product, UpdateProductRequest>().ReverseMap();
            CreateMap<Product, UpdateProductStockRequest>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, CreateOrderRequest>()
                .ForMember(dest => dest.Mode, action => action.Ignore())
                .ReverseMap();
        }
    }
}
