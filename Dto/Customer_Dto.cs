using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
   
   public class Customer_Dto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public  Customer_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<Customer_Dto,Customer_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<Customer_Tbl>(this);
        }


        public static Customer_Dto DalToDto(Customer_Tbl c)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Customer_Tbl, Customer_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<Customer_Dto>(c);
        }
    }
}
