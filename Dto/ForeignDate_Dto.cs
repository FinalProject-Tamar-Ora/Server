using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
   public class ForeignDate_Dto
    {


        public int ForeignDateId { get; set; }
        public int HebrewDateId { get; set; }
        public Nullable<System.DateTime> ForeignDate { get; set; }
        public ForeignDate_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<ForeignDate_Dto, ForeignDate_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<ForeignDate_Tbl>(this);
        }


        public static ForeignDate_Dto DalToDto(ForeignDate_Tbl f) 
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<ForeignDate_Tbl, ForeignDate_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ForeignDate_Dto>(f);
        }
    }
}
