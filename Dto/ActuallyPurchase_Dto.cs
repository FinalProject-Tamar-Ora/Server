using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
   public class ActuallyPurchase_Dto
    {
        public int ActuallyPurchaseId { get; set; }
        public int PurchasesHistoryId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> PurchasePrognosisId { get; set; }


        //public ActuallyPurchase_Dto(PurchasePrognosis_Dto p)
        //{
        //    try
        //    {
        //        PurchasesHistoryId = p.PurchasesHistoryId;
        //        ProductId = p.ProductId;
        //        Amount = p.Amount;
        //        PurchasePrognosisId = p.PurchasePrognosisId;
        //    }
        //    catch
        //    {

        //    }

        //}

        public ActuallyPurchase_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<ActuallyPurchase_Dto,ActuallyPurchase_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<ActuallyPurchase_Tbl>(this);
        }


        public static ActuallyPurchase_Dto DalToDto(ActuallyPurchase_Tbl a)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<ActuallyPurchase_Tbl, ActuallyPurchase_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ActuallyPurchase_Dto>(a);
        }

        public void Copy(ActuallyPurchase_Tbl p)
        {
            //p.ActuallyPurchaseId = ActuallyPurchaseId;
            //p.PurchasePrognosisId = PurchasePrognosisId;
            p.ProductId = ProductId;
            p.PurchasesHistoryId = PurchasesHistoryId;
            p.Amount = (int)Amount;
        }
    }
}
