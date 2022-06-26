using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
   public class HebrewDate_Dto
    {
        public int HebrewDateId { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public HebrewDate_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<HebrewDate_Dto, HebrewDate_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<HebrewDate_Tbl>(this);
        }


        public static HebrewDate_Dto DalToDto(HebrewDate_Tbl t)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<HebrewDate_Tbl, HebrewDate_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<HebrewDate_Dto>(t);
        }



    }
}
