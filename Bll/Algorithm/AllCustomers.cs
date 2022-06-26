using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace Bll.Algorithm
{
   public class AllCustomers
    {
        CustomerDB dbc = new CustomerDB();
        PurchasesHistoryDB dbph = new PurchasesHistoryDB();
        ActuallyPurchaseDB dba = new ActuallyPurchaseDB();
        BehaviourPatternBuilder bpb = new BehaviourPatternBuilder();
        readonly int year = GeneralFunctions.Get_CurrentYear() - 1;
        //פונקציה העוברת על לקוחות המערכת ובונה דפוסי התנהגות למוצרים,הפונקציה תזומן בסיום כל שנה
        public void BehaviourPatternBuilderForAllCustomers()
        {
            //עבור על כל הלקוחות 
            foreach (Customer_Tbl c in dbc.GetAllCustomers())
                bpb.CustomerBehaviourPatternsBuilder(GetCustomerProducts(c.CustomerId), c.CustomerId);
        }

        public List<ActuallyPurchase_Tbl> GetCustomerProducts(int custId)
        {
            List<ActuallyPurchase_Tbl> lst = new List<ActuallyPurchase_Tbl>();
            foreach(PurchasesHistory_Tbl p in GetPurchasesByYearAndCustId(custId).ToList())
            { lst.AddRange(dba.GetActuallyPurchasesByPurchaseHistoryId(p.PurchasesHistoryId)); }
            return lst;
        }

        public List<PurchasesHistory_Tbl> GetPurchasesByYearAndCustId(int custId)//לקבל את כל הרכישות של הלקוח
        {
           return dbph.GetPurchasesHistoryByYear(year, custId);
        }


    }
}
