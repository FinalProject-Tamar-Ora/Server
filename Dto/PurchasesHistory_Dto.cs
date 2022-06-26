using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
   public class PurchasesHistory_Dto
    {
        public int PurchasesHistoryId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string Description { get; set; }
        public int HebrewDateId { get; set; }
        public bool BelongToCurrentYear { get; set; }


        public PurchasesHistory_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<PurchasesHistory_Dto, PurchasesHistory_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<PurchasesHistory_Tbl>(this);
        }


        public static PurchasesHistory_Dto DalToDto(PurchasesHistory_Tbl p)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<PurchasesHistory_Tbl, PurchasesHistory_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<PurchasesHistory_Dto>(p);
        }
      

    }
}
