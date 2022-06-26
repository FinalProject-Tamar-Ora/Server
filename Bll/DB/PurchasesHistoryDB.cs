using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
    public class PurchasesHistoryDB
    {
        HebrewDateDB dbh = new HebrewDateDB();// שליפה ושמירה בטבלתHebrewDate 
        Shopper_DBEntities1 db = new Shopper_DBEntities1();
        public void AddNewPurchase(PurchasesHistory_Tbl newPurchase)
        {
            db.PurchasesHistory_Tbl.Add(newPurchase);
            db.SaveChanges();
        }

        public int GetPurchaseHistoryIdByDateTime(DateTime dt)
        {
            return db.PurchasesHistory_Tbl.Where(p => p.PurchaseDate == dt).FirstOrDefault().PurchasesHistoryId;
        }
        public RequestResult GetAllPurchasesHistoryByCustomerId(int custId)
        {
            List<PurchasesHistory_Dto> lst = new List<PurchasesHistory_Dto>();
            foreach (var item in db.PurchasesHistory_Tbl.ToList())
            {
                if(item.CustomerId==custId )
                {
                    lst.Add(PurchasesHistory_Dto.DalToDto(item));
                }
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };
        }
        public List<DateTime?> GetLastPurchaseDatesOfProdId(int prodId, int custId)
        {
            return db.ProductPurchasesDatesDesc(prodId, custId).ToList();
        }

        public List<PurchasesHistory_Tbl> GetPurchasesByCustIdAndCurrDate(int custId, DateTime dt,int phId)
        {
            int hdId = db.ForeignDate_Tbl.Where(p => p.ForeignDate == dt).FirstOrDefault().HebrewDateId;
            int m = db.HebrewDate_Tbl.Where(p => p.HebrewDateId == hdId).FirstOrDefault().Month;
            
            return db.PurchasesHistory_Tbl.Where(p =>p.PurchasesHistoryId!=phId && p.CustomerId == custId 
                  && db.HebrewDate_Tbl.Where(b=>b.HebrewDateId==p.HebrewDateId).FirstOrDefault().Month==m).ToList();
        }

        public DateTime GetDate(int phId)
        {
            return (DateTime)db.PurchasesHistory_Tbl.Where(p => p.PurchasesHistoryId == phId).FirstOrDefault().PurchaseDate;
        }

        public int GetHebrewDateId(int phId)
        {
           return db.PurchasesHistory_Tbl.Where(p => p.PurchasesHistoryId == phId).FirstOrDefault().HebrewDateId;
        }


        public void UpdatePurchasesHistory(int custId)
        {
            List<PurchasesHistory_Tbl> list = db.PurchasesHistory_Tbl.Where(p => p.CustomerId == custId && p.BelongToCurrentYear == true).ToList();
            foreach (PurchasesHistory_Tbl ph in list)
            {
                ph.BelongToCurrentYear = false;
                db.SaveChanges();
            }
         
        }
        public List<PurchasesHistory_Tbl> GetPurchasesHistoryByYear(int hebrewYear,int custId)
        {
            int first = dbh.GetFirstHebrewDateIdOfYear(hebrewYear);
            int last = dbh.GetLastHebrewDateIdOfYear(hebrewYear);
            return GetPurchasesByHebrewDateId(custId, first, last);
        }
        public List<PurchasesHistory_Tbl> GetPurchasesByHebrewDateId(int custId,int first,int last)
        {
           return db.PurchasesHistory_Tbl.Where(p => p.CustomerId == custId && p.HebrewDateId>= first && p.HebrewDateId <= last).ToList();
        }
        public int GetNewPurchaseId(int custId)
        {
            return db.PurchasesHistory_Tbl.Where(o=>o.CustomerId==custId).OrderByDescending(p => p.PurchaseDate).FirstOrDefault().PurchasesHistoryId;
        }
    }
}
