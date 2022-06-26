using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class BehaviourPattern_Dto
    {
        static Shopper_DBEntities1 db = new Shopper_DBEntities1();
        public int BehaviourPatternId { get; set; }
        public int Status { get; set; }
        public Nullable<int> EveryXMonthsBuy { get; set; }
        public Nullable<System.DateTime> LastPurchaseDate { get; set; }
        public Nullable<int> Amount { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string NameOfEvent { get; set; }
        public Nullable<int> XTimesOnMonthBuy { get; set; }
        public Nullable<bool> IsNewProduct { get; set; }
        public Nullable<int> TimesOfCancelation { get; set; }
        public Nullable<int> TimesOfAmountChange { get; set; }
        public Nullable<int> TimesNotFoundInPrognosis { get; set; }

        public static void Copy(BehaviourPattern_Tbl source, BehaviourPattern_Tbl dest)
        {
            dest.BehaviourPatternId = source.BehaviourPatternId;
            dest.Status = source.Status;
            dest.TimesOfCancelation = source.TimesOfCancelation;
            dest.CustomerId = source.CustomerId;
            dest.ProductId = source.ProductId;
            dest.TimesNotFoundInPrognosis = source.TimesNotFoundInPrognosis;
            dest.TimesOfAmountChange = source.TimesOfAmountChange;
            dest.TimesOfCancelation = source.TimesOfCancelation;
            dest.IsNewProduct = source.IsNewProduct;
            dest.NameOfEvent = source.NameOfEvent;
            dest.EveryXMonthsBuy = source.EveryXMonthsBuy;
            dest.XTimesOnMonthBuy = source.XTimesOnMonthBuy;
            dest.LastPurchaseDate = source.LastPurchaseDate;
            dest.Amount = source.Amount;
            db.SaveChanges();

        }
        public BehaviourPattern_Tbl DtoTODal()
        {
            var config = new MapperConfiguration(cfg =>
                   cfg.CreateMap<BehaviourPattern_Dto, BehaviourPattern_Tbl>()
               );
            var mapper = new Mapper(config);
            return mapper.Map<BehaviourPattern_Tbl>(this);
        }


        public static BehaviourPattern_Dto DalToDto(BehaviourPattern_Tbl b)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<BehaviourPattern_Tbl, BehaviourPattern_Dto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<BehaviourPattern_Dto>(b);
        }



    }
}
