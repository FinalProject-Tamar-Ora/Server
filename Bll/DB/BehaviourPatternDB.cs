using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Dal;
namespace Bll
{
    class BehaviourPatternDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();

        public void AddNewBehaviourPattern(BehaviourPattern_Tbl b)
        {
            db.BehaviourPattern_Tbl.Add(b);
            db.SaveChanges();
        }

        public void UpdateBehaviourPattern(BehaviourPattern_Tbl source,BehaviourPattern_Tbl dest)
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

        public BehaviourPattern_Tbl GetBehaviourPatternByProdIdCustIdAndAmount(int custId,int prodId,int amount)
        {
           return  db.BehaviourPattern_Tbl.Where(p => p.ProductId == prodId && p.CustomerId == custId && p.Amount==amount).FirstOrDefault();
        }
        public List<BehaviourPattern_Tbl> GetBehaviourPatternLstByCustId(int custId)
        {
            return db.BehaviourPattern_Tbl.Where(p => p.CustomerId == custId).ToList();
        }
        public BehaviourPattern_Tbl GetLowStatusBehaviourPattern(int custId,int prodId)
        {
            return db.BehaviourPattern_Tbl.Where(p => p.CustomerId == custId && p.ProductId == prodId).OrderBy(o => o.Status).FirstOrDefault();
        }

    }
}
