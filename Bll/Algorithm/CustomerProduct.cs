using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
   public class CustomerProduct
    {
        PurchasesHistoryDB dbph = new PurchasesHistoryDB();//שליפה ושמירה בטבלת PurchasesHistory
        HebrewDateDB dbh = new HebrewDateDB();// שליפה ושמירה בטבלתHebrewDate 
        public List<ActuallyPurchase_Tbl> ActPurLst { get; set; }//רשימת עזר להכנסת מוצרים שנרכשו בפועל במהלך כל השנה
        public int[] CounterArr { get; set; }//מונה לכל חודש כמה פעמים המוצר נרכש
        public int[] SumArr { get; set; }//סוכם את הכמות הנרכשת של המוצר עבור כל חודש  במהלך השנה
        public List<DateTime>[] DatesArr { get; set; }//עבור כל חודש יוצר רשימה עם תאריכי הקניה לחודש זה
        public List<int> ExceptionalMonthsLst { get; set; }//רשימה המחזיקה חודשים חריגים בשנה אחרת
        public int[] AmountPerPurchaseArr { get; set; }//מערך תוצאות של מערך הסוכם חלקי מערך המונה
        public int CustomerId { get; set; } //מחזיק את קוד הלקוח המטופל כעת
        public int ProductId { get; set; }
        public CustomerProduct() { }//פעולה בונה ריקה
        public CustomerProduct(int custId)//פעולה בונה המקבלת קוד לקוח ומאתחלת את הפרופרטיס
        {
            ActPurLst = new List<ActuallyPurchase_Tbl>();
            CounterArr = new int[14];
            SumArr = new int[14]; 
            DatesArr = new List<DateTime>[14];
            ExceptionalMonthsLst = new List<int>();
            AmountPerPurchaseArr = new int[14];
            CustomerId = custId;
        }

        //בניית מערך הרשימות
        public void DatesArrBuilder()
        {
            for (int i = 0; i < DatesArr.Length; i++)
            {
                DatesArr[i] = new List<DateTime>();

            }
        }
        //ריקון רשימת חריגים
       public void ExceptionalMonthsLstClear()
        {
            ExceptionalMonthsLst.Clear();
        }
        //ריקון מערך הרשימות
        public void DatesArrClear()
        {
            for (int i = 0; i < DatesArr.Length; i++)
            {
                DatesArr[i].Clear();

            }
        }
        ///  איפוס מערכים
        public void ZeroArrays( int num)
        {
            CounterArr[0] = -1;
              AmountPerPurchaseArr[0] = -1;
            SumArr[0] = -1;
            for (int i = 1; i < CounterArr.Length; i++)
            {

                if (GeneralFunctions.Get_HebrewCalendar().IsLeapYear(GeneralFunctions.Get_CurrentYear() - num) == false)
                {
                    if (i == 13)
                    {
                        CounterArr[i] = -1;
                        SumArr[i] = -1;
                        AmountPerPurchaseArr[i] = -1;
                    }

                    else
                    {
                        CounterArr[i] = 0;
                        SumArr[i] = 0;
                        AmountPerPurchaseArr[i] = 0;
                    }

                }
                else
                {
                    CounterArr[i] = 0;
                    SumArr[i] = 0;
                    AmountPerPurchaseArr[i] = 0;
                }


            }

        }

        //פונקציה מעדכנת את רכישות מהשנה האחרונה כלא שייכות לשנה הבאה
        public void UpdatePurchasesHistory()
        {
            dbph.UpdatePurchasesHistory(CustomerId);
        }


        //פונקציה מקבלת רשימת רכישות של המוצר בשנה מסוימת מעדכנת את המערכים ומחזירה את תאריך רכישה אחרון שהמוצר נרכש
        public DateTime ArraysFiller(List<ActuallyPurchase_Tbl> lst)
        {

            HebrewDate_Tbl hd;
            DateTime lastDateBuy = new DateTime(2000, 1, 1);// תאריך קניה אחרון של מוצר
            foreach (ActuallyPurchase_Tbl a in lst)
            {

                //מציאת תאריך לועזי 
                DateTime d = dbph.GetDate(a.PurchasesHistoryId);
                if (d > lastDateBuy)
                    lastDateBuy = d;
                int id = dbph.GetHebrewDateId(a.PurchasesHistoryId);
                //מציאת תאריך עברי
                hd = dbh.GetHebrewDateByHebrewDateId(id);             //גישה למערכים ע"פ תאריך עברי
                CounterArr[hd.Month]++;
                SumArr[hd.Month] += a.Amount;
                DatesArr[hd.Month].Add(d);
            }
            return lastDateBuy;
        }

      
    }
}
