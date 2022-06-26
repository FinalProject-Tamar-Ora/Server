using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
  public  class ProductCategory_Dto
    {
        public int ProductCategoryId { get; set; }
        public Nullable<int> ParentProductCategoryId { get; set; }
        public string Name { get; set; }



        public ProductCategory_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<ProductCategory_Dto, ProductCategory_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<ProductCategory_Tbl>(this);
        }


        public static ProductCategory_Dto DalToDto(ProductCategory_Tbl c)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<ProductCategory_Tbl,ProductCategory_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ProductCategory_Dto>(c);
        }





    }
}
