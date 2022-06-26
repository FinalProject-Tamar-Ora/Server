using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
  public  class PurchasePrognosisDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();


        public RequestResult GetAllPurchasePrognosis()
        {

            List<PurchasePrognosis_Dto> lst = new List<PurchasePrognosis_Dto>();
            foreach (var item in db.PurchasePrognosis_Tbl.ToList())
            {
                lst.Add(PurchasePrognosis_Dto.DalToDto(item));

            }

            return new RequestResult() { Data = lst, Message = "success", Status = true };

        }
        public RequestResult GetPurchasePrognosisById(int purchasePrognosisId)
        {


            List<PurchasePrognosis_Dto> lst = new List<PurchasePrognosis_Dto>();
            foreach (var item in db.PurchasePrognosis_Tbl.ToList())
            {
                if (item.PurchasePrognosisId==purchasePrognosisId)
                      lst.Add(PurchasePrognosis_Dto.DalToDto(item));
            }
            return new RequestResult() { Data = lst, Message = "success", Status = true };

        }
        //public void UpdateList(List<PurchasePrognosis_Dto> lst)
        //{
        //    ActuallyPurchase_Dto x;
        //    foreach(PurchasePrognosis_Dto p in lst)
        //    {
        //        x = new ActuallyPurchase_Dto(p);
        //        db.ActuallyPurchase_Tbl.Add(x.DtoTODal());
        //        db.SaveChanges();
        //    }
        //}
        //public void AddNewProduct(PurchasePrognosis_Dto p)
        //{
        //    ActuallyPurchase_Dto b = new ActuallyPurchase_Dto(p);
        //    db.ActuallyPurchase_Tbl.Add(b.DtoTODal());
        //    db.SaveChanges();
        //}


        public List<PurchasePrognosis_Tbl> GetLastPrognosisLst(int custId)
        {
            int currentPurchase = db.PurchasesHistory_Tbl.Where(p => p.CustomerId == custId)
              .OrderByDescending(k => k.PurchaseDate).FirstOrDefault().PurchasesHistoryId;
            List<PurchasePrognosis_Tbl> lst = db.PurchasePrognosis_Tbl.Where(p => p.PurchasesHistoryId == currentPurchase).ToList();
            return lst;
        }
        public void AddPrognosisLst(List<PurchasePrognosis_Dto> prognosisLst)
        {
            foreach (PurchasePrognosis_Dto p in prognosisLst)
            {
                db.PurchasePrognosis_Tbl.Add(p.DtoTODal());
                db.SaveChanges();

            }
        }
    }
}
 