using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
   public class ActuallyPurchaseDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();
        ProductDB dbp = new ProductDB();
        PurchasesHistoryDB dbph = new PurchasesHistoryDB();
        public List<ActuallyPurchase_Tbl> GetActuallyPurchasesByPurchaseHistoryId(int purchasesHistoryId)
        {
            List<ActuallyPurchase_Tbl> list = db.ActuallyPurchase_Tbl.Where(p => p.PurchasesHistoryId == purchasesHistoryId).ToList();
            return list;
        }
        public int GetAmountByProdIdAndphId(int prodId,int phId )
        {
           return db.ActuallyPurchase_Tbl.Where(p => p.ProductId == prodId && p.PurchasesHistoryId == phId).FirstOrDefault().Amount;
        }
        public List<ActuallyPurchase_Dto> GetActuallyPurchasesByPurchasesHistoryId(int purchasesHistoryId)
        {
            List<ActuallyPurchase_Dto> lst = new List<ActuallyPurchase_Dto>();
            List<ActuallyPurchase_Tbl> list = db.ActuallyPurchase_Tbl.Where(p => p.PurchasesHistoryId == purchasesHistoryId).ToList();
            foreach (ActuallyPurchase_Tbl a in list)
            {
                ActuallyPurchase_Dto ap = ActuallyPurchase_Dto.DalToDto(a);
                ap.Name = dbp.GetNameOfProductByProdId(a.ProductId);
                ap.Description = dbp.GetDescriptionOfProductByProdId(a.ProductId);
                ap.Image = dbp.GetImageOfProductByProdId(a.ProductId);
                lst.Add(ap);
            }
            return lst;
        }
        public List<ActuallyPurchase_Tbl> AddActuallyLst(List<ActuallyPurchase_Dto> actuallyLst,int custId)
        {
            int lastPurchaseId = dbph.GetNewPurchaseId(custId);
            List<ActuallyPurchase_Tbl> lst = new List<ActuallyPurchase_Tbl>();
            foreach (ActuallyPurchase_Dto a in actuallyLst)
            {
                a.PurchasesHistoryId = lastPurchaseId;
                ActuallyPurchase_Tbl c = new ActuallyPurchase_Tbl();
                a.Copy(c);
                db.ActuallyPurchase_Tbl.Add(c);
                lst.Add(c);
              //db.SaveChanges();
            }
            return lst; 
        }

        public List<ActuallyPurchase_Tbl> AllPurchasesOfProductIdOneYear(int custId,int prodId, int year)
        {
            List<ActuallyPurchase_Tbl> list = new List<ActuallyPurchase_Tbl>();
            List<PurchasesHistory_Tbl> lstph = dbph.GetPurchasesHistoryByYear(year,custId);
            foreach (var ph in lstph)
            {
                List<ActuallyPurchase_Tbl> lst = db.ActuallyPurchase_Tbl.Where(p => p.PurchasesHistoryId == ph.PurchasesHistoryId).ToList();
                list.AddRange(lst.Where(p => p.ProductId == prodId).ToList());
            }
            return list;
        }

    }
}
