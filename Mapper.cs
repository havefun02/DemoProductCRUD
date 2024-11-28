using AutoMapper;

namespace DemoCRUD
{
    public class Mapper:Profile
    {
        public Mapper() {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductResponse>();

            CreateMap<UpdateProductRequest, Product>();
        }

    }
}
