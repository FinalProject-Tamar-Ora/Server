using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
  public  class Product_Dto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public Product_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<Product_Dto, Product_Tbl > ()
               );
            var mapper = new Mapper(config);
            return mapper.Map<Product_Tbl>(this);
        }


        public static Product_Dto DalToDto(Product_Tbl p)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Product_Tbl, Product_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<Product_Dto>(p);
        }

    }
}
