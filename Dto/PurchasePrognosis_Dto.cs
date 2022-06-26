using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
  public  class PurchasePrognosis_Dto
    {
        public int PurchasePrognosisId { get; set; }
        public int PurchasesHistoryId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> Amount { get; set; }
        public PurchasePrognosis_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<PurchasePrognosis_Dto,PurchasePrognosis_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<PurchasePrognosis_Tbl>(this);
        }


        public static PurchasePrognosis_Dto DalToDto(PurchasePrognosis_Tbl p)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<PurchasePrognosis_Tbl, PurchasePrognosis_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<PurchasePrognosis_Dto>(p);
        }

        public void Copy(PurchasePrognosis_Tbl p)
        {
            p.PurchasePrognosisId=PurchasePrognosisId;
            p.ProductId = ProductId;
            p.PurchasesHistoryId = PurchasesHistoryId;
            p.Amount = (int)Amount;
        }


    }
}
