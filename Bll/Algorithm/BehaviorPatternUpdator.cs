using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll.Algorithm
{
    public class BehaviorPatternUpdator
    {
        PurchasePrognosisDB dbp = new PurchasePrognosisDB();//שליפות מטבלת PurchasePrognosis
        ActuallyPurchaseDB dba = new ActuallyPurchaseDB();//שליפות מטבלת  ActuallyPurchase
        BehaviourPatternDB dbbp = new BehaviourPatternDB();//שליפות מטבלת BehaviourPattern
        PurchasesHistoryDB dbph = new PurchasesHistoryDB();//שליפות מטבלת PurchasesHistory
        public int PreviousPurchaseDates { get; set; }//מספר תאריכי קניה קודמים
        public List<PurchasePrognosis_Tbl> BasketProg { get; set; }//סל קניות המוצע ע"י המערכת
        public List<ActuallyPurchase_Tbl> BasketAct { get; set; }//סל קניות הנרכש בפועל 

        readonly static DateTime lastPurchaseDate = new DateTime(2021, 12, 20);
        public BehaviorPatternUpdator()//פעולה בונה המאתחלת נתונים
        {
            BasketAct = new List<ActuallyPurchase_Tbl>();
            BasketProg = new List<PurchasePrognosis_Tbl>();
            PreviousPurchaseDates = 4;
        }


        //המערכת  משווה בין 2 סלי הקניות-רשימת מוצרים שנערכה ע"י המערכת ורשימת המוצרים שאושרה ע"י הלקוח
        // על מנת לזהות מוצרים שדפוס התנהגותם עומד להשתנות
        public void CompareBetweenPrognosisAndActually(int custId, List<ActuallyPurchase_Dto> lstFromReact)
        {
            BasketProg = dbp.GetLastPrognosisLst(custId);//לקבל את סל הקניות שהמערכת מציעה
            BasketAct = dba.AddActuallyLst(lstFromReact, custId);//שמירת סל הקניות הנרכש בפועל 
            //עבור על המוצרים שהתקבלו מהלקוח
            foreach (ActuallyPurchase_Tbl a in BasketAct)
            {
                bool appearInProg = false;
                //חפש מוצר אם קיים במוצרים שהאלגוריתם הציע 
                foreach (PurchasePrognosis_Tbl pg in BasketProg)
                {
                    if (a.ProductId == pg.ProductId)
                    {
                        appearInProg = true;
                        FoundInBasketProg(custId, a, pg);
                        break;
                    }
                }
                //המוצר לא נמצא ברשימת המוצרים שהמערכת הציעה
                if (appearInProg == false)
                {
                    BehaviourPattern_Tbl b = dbbp.GetLowStatusBehaviourPattern(custId, a.ProductId);
                    if (b == null)
                        NewProduct(a, custId);
                    else NotFoundInBasketProg(b, custId);

                }
            }
        }


        //טיפול במוצר חדש
        public void NewProduct(ActuallyPurchase_Tbl a, int custId)
        {
            BehaviourPattern_Tbl newOne = new BehaviourPattern_Tbl
            {
                ProductId = a.ProductId,
                CustomerId = custId,
                LastPurchaseDate = lastPurchaseDate,
                IsNewProduct = true,
                Status = Classification.Unknown,
                Amount = a.Amount,
                TimesOfCancelation = 0,
                TimesNotFoundInPrognosis = 1,
                TimesOfAmountChange = 0,
                EveryXMonthsBuy = 0,
                XTimesOnMonthBuy = 0,
                NameOfEvent = " "


            };
            dbbp.AddNewBehaviourPattern(newOne);
        }
        //טיפול במוצר שלא נמצא בסל הקניות שהמערכת מציעה אבל נמצא בסל הקניות הנרכש בפועל
        public void NotFoundInBasketProg(BehaviourPattern_Tbl b, int custId)
        {
            BehaviourPattern_Tbl temp = new BehaviourPattern_Tbl();
            BehaviourPattern_Dto.Copy(b, temp);

            // מספר הפעמים שמוצר לא הופיע בהצעת הקניה  קטן מ4 
            if (temp.TimesNotFoundInPrognosis < PreviousPurchaseDates)
                temp.TimesNotFoundInPrognosis += 1;
            else
            {
                //אם סטטוס המוצר לא מוגדר וקנו אותו לפחות 4 פעמים אז הוא כבר לא חדש
                if (temp.Status == (int)Classification.Unknown)
                    temp.IsNewProduct = false;
                temp.TimesOfAmountChange = 0;
                temp.TimesNotFoundInPrognosis = 0;
                temp.TimesOfCancelation = 0;
                temp.XTimesOnMonthBuy = 0;
                temp.EveryXMonthsBuy = 0;
                temp.NameOfEvent = " ";
                temp = StatusForProd(temp, custId);
            }
            temp.LastPurchaseDate = lastPurchaseDate;//עדכון תאריך קניה אחרון למוצר לתאריך הנוכחי
            dbbp.UpdateBehaviourPattern(temp, b);
        }


        //טיפול במוצר הנרכש בפועל וגם מוצע ע"י המערכת 
        public void FoundInBasketProg(int custId, ActuallyPurchase_Tbl ap, PurchasePrognosis_Tbl pp)
        {
            BehaviourPattern_Tbl b = dbbp.GetBehaviourPatternByProdIdCustIdAndAmount(custId, ap.ProductId, pp.Amount);
            BehaviourPattern_Tbl temp = new BehaviourPattern_Tbl();
            BehaviourPattern_Dto.Copy(b, temp);
            //אם המוצר בוטל
            if (ap.Amount == 0)
            {
                if (temp.TimesOfCancelation < 2)
                {
                    temp.TimesOfCancelation += 1;
                    dbbp.UpdateBehaviourPattern(temp, b);
                }
            }
            //אם הכמות השתנתה  
            else if (ap.Amount != pp.Amount)
            {

                if (temp.TimesOfAmountChange < 2)
                    temp.TimesOfAmountChange += 1;
                else
                {
                    temp.TimesOfAmountChange = 0;
                    temp.Amount = ap.Amount;
                }
                temp.LastPurchaseDate = lastPurchaseDate;//עדכון תאריך קניה אחרון למוצר לתאריך הנוכחי
                dbbp.UpdateBehaviourPattern(temp, b);
            }
            else
            {
                temp.LastPurchaseDate = lastPurchaseDate;//עדכון תאריך קניה אחרון למוצר לתאריך הנוכחי
                dbbp.UpdateBehaviourPattern(temp, b);
            }
        }

        //פונקציה המחזירה את הפרש החודשים בין 2 תאריכים עבריים
        public int Difference(DateTime first, DateTime last)
        {
            int currYear = GeneralFunctions.Get_HebrewCalendar().GetYear(first);
            int lastPurchaseDateYear = GeneralFunctions.Get_HebrewCalendar().GetYear(last);
            int currMonth = GeneralFunctions.Get_HebrewCalendar().GetMonth(first);
            int lastPurchaseDateMonth = GeneralFunctions.Get_HebrewCalendar().GetMonth(last);
            int monthsApart = 12 * (currYear - lastPurchaseDateYear) + currMonth - lastPurchaseDateMonth;
            return monthsApart;
        }

        //פונקציה המעדכנת דפוסי התנהגות 
        public BehaviourPattern_Tbl StatusForProd(BehaviourPattern_Tbl b, int currentCustomerId)
        {
            DateTime[] arr = LastDates(b.ProductId, currentCustomerId);//לקבל במערך תאריכים אחרונים של רכישת המוצר
            int numOfMonths;//מספר חודשים ב4 תאריכי קניה אחרונים
            int diffBetweenPurchasesGreaterOne;//מספר פעמים שהפרש בין החודשים גדולמ1
            int countDates;//מונה בפועל כמה תאים במערך מאותחלים עם תאריכים
            bool isOnceYear;//האם זה מוצר שנתי
            DatesArrayInvestigation(arr, out countDates, out isOnceYear, out diffBetweenPurchasesGreaterOne, out numOfMonths);
            int range = Difference(arr[0], arr[countDates - 1]);
            bool sameSeason = GeneralFunctions.SameSeasons(arr);

            if (diffBetweenPurchasesGreaterOne > 1)
                return DataPlacing(b, Classification.Frequently, range, numOfMonths, countDates,currentCustomerId,b.ProductId);

            if (diffBetweenPurchasesGreaterOne == 0 || diffBetweenPurchasesGreaterOne == 1)
                return DataPlacing(b, Classification.Monthly, range, numOfMonths, countDates,currentCustomerId,b.ProductId);

            return b;

        }
        public BehaviourPattern_Tbl DataPlacing(BehaviourPattern_Tbl b, Classification c, int range, int numOfMonths, int countDates,int custId,int prodId)
        {
            b.Status = c;
            b.EveryXMonthsBuy = ((int)Math.Round((double)(range + 1) / numOfMonths));
            b.XTimesOnMonthBuy = (int)Math.Round((double)countDates / numOfMonths);
            b.Amount = AvarageAmount(prodId, custId);
            return b;
        }
        public int AvarageAmount(int prodId,int custId)
        {
            double sum = 0;
            List<DateTime?> lst = dbph.GetLastPurchaseDatesOfProdId(prodId, custId);
            foreach(DateTime d in lst)
            {
                int phId = dbph.GetPurchaseHistoryIdByDateTime(d);
                sum += dba.GetAmountByProdIdAndphId(prodId, phId);
            }
            return (int)Math.Round(sum / (lst.Count));
        }

        //חקירה מערך תאריכים
        public void DatesArrayInvestigation(DateTime[] arr, out int countDates, out bool isOnceYear, out int diffBetweenMnthGreaterOne, out int numOfMonths)
        {
            numOfMonths = 1;//מספר חודשים ב4 תאריכי קניה אחרונים
            countDates = 1;//מונה בפועל כמה תאים במערך מאותחלים עם תאריכים
            diffBetweenMnthGreaterOne = 0;//מספר פעמים שהפרש בין החודשים גדולמ1
            isOnceYear = false;//האם זה מוצר שנתי
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] != null && arr[i + 1] != null)
                {
                    countDates++;
                    diffBetweenMnthGreaterOne = Difference(arr[i], arr[i + 1]);
                    if (diffBetweenMnthGreaterOne >= 12)
                        isOnceYear = true;
                    if (diffBetweenMnthGreaterOne > 0)
                        numOfMonths++;
                    if (diffBetweenMnthGreaterOne > 1)
                        diffBetweenMnthGreaterOne++;
                }
                else if (arr[i + 1] == null)
                {

                    diffBetweenMnthGreaterOne = (12 * (GeneralFunctions.Get_HebrewCalendar().GetYear(lastPurchaseDate) - GeneralFunctions.Get_HebrewCalendar().GetYear(arr[i])) + GeneralFunctions.Get_HebrewCalendar().GetMonth(lastPurchaseDate) - GeneralFunctions.Get_HebrewCalendar().GetMonth(arr[i]));
                    if (diffBetweenMnthGreaterOne >= 12)
                        isOnceYear = true;
                }

            }
        }

        //הפונקציה מחזירה במערך  תאריכי קניה האחרונים של מוצר מסוים
        public DateTime[] LastDates(int prodId, int custId)
        {
            DateTime[] arr = new DateTime[PreviousPurchaseDates];
            int ind = 0;

            List<DateTime?> lst = dbph.GetLastPurchaseDatesOfProdId(prodId, custId);
            foreach (DateTime? dt in lst)
            {
                if (ind == PreviousPurchaseDates)
                    break;
                arr[ind++] = (DateTime)dt;

            }
            return arr;

        }

    }
}
