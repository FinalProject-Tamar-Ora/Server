using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll.Algorithm
{
    public class BasketBuilderDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();
        ProductDB dbp = new ProductDB();
        public RequestResult GetPurchaseOffer(int customerId)
        {
            PurchaseOffer pf = new PurchaseOffer();
            List<PurchasePrognosis_Dto> lst = pf.NewPurchase(customerId);
            return new RequestResult() { Data = lst, Message = "success", Status = true };
        }

        public void ActuallyPurcase(int customerId, List<ActuallyPurchase_Dto> lst)
        {
            BehaviorPatternUpdator up = new BehaviorPatternUpdator();
            up.CompareBetweenPrognosisAndActually(customerId, lst);
        }
        public RequestResult GetFinallyPurchase(int customerId)
        {
            PurchasesHistory_Tbl lastP = db.PurchasesHistory_Tbl.Where(p => p.CustomerId == customerId).OrderByDescending(o => o.PurchaseDate).FirstOrDefault();
            List<ActuallyPurchase_Dto> lst = new List<ActuallyPurchase_Dto>();
            foreach (ActuallyPurchase_Tbl a in db.ActuallyPurchase_Tbl.ToList())
                if (a.PurchasesHistoryId == lastP.PurchasesHistoryId)
                    lst.Add(ActuallyPurchase_Dto.DalToDto(a));
            foreach(ActuallyPurchase_Dto a in lst)
            {
                a.Name = dbp.GetNameOfProductByProdId(a.ProductId);
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };

        }
    }
}
