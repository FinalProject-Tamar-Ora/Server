using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;

namespace Bll
{
    public class BehaviourPatternBuilder
    {
        ActuallyPurchaseDB dba = new ActuallyPurchaseDB();//שליפה ושמירה בטבלת ActuallyPurchase
        ForeignDateDB dbf = new ForeignDateDB();//שליפה ושמירה בטבלת ForeignDate
        HebrewDateDB dbh = new HebrewDateDB();//שליפה ושמירה בטבלת HebrewDate
        BehaviourPatternDB dbbp = new BehaviourPatternDB();//שליפה ושמירה בטבלת BehaviourPattern
        PurchasesHistoryDB dbph = new PurchasesHistoryDB();//שליפה ושמירה בטבלת PurchasesHistory
        CustomerDB dbc = new CustomerDB();//שליפה ושמירה בטבלת Customer
        readonly static string summer = "קיץ";
        readonly static string winter = "חורף";
        readonly static string summerVacation = "החופש הגדול";
        readonly static int frequently = 1;
        readonly static int winterSeason = 3;
        readonly static int summerSeason = 2;
        readonly static int summerMonthsNum = 7;
        readonly static int winterMonthsNum = 6;
        readonly static int summerVacMonthsNum = 2;
        readonly static int year = GeneralFunctions.Get_CurrentYear();
        public CustomerProduct CustomerProductA { get; set; }//מוצר הלקוח בשנה האחרונה
        public CustomerProduct CustomerProductB { get; set; }//מוצר הלקוח בשנה שעברה




        //////////////////////////////תהליך עיקרי בקביעת דפוסי התנהגות למוצרי לקוח בודד///////////////////////////

        /// הפונקציה עוברת על המוצרים שהתקבלו ועבור כל מוצר מחשבת תדירות וכמות ולאחר מכן מטפלת בחודשים החריגים בכמות
        public void CustomerBehaviourPatternsBuilder(List<ActuallyPurchase_Tbl> productsLst, int custId)
        {
            InitCustomerProduct(custId);
            CustomerProductA.ActPurLst = productsLst;
            //קבץ את כלל המוצרים לפי קוד מוצר על מנת לטפל בכל תאריכי הקניה של המוצר בפעם אחת
            var groupsByProductId = CustomerProductA.ActPurLst.GroupBy(p => p.ProductId).ToList();
            foreach (var group in groupsByProductId)
            {
                SetProdId(group.FirstOrDefault().ProductId);
                ResetArrays(); ResetLists();
                DateTime lastForeignPurchaseDate = ProductPurchases(group.ToList());//מילוי מערכים
                int id = dbf.GetHebrewDateIdByForeignDate(lastForeignPurchaseDate);//מזהה תאריך עברי של תאריך רכישה אחרון של המוצר
                HebrewDate_Tbl lastHebrewPurchaseDate = dbh.GetHebrewDateByHebrewDateId(id);//תאריך עברי של הרכישה האחרונה של המוצר
                CalculateFrequency(lastHebrewPurchaseDate, lastForeignPurchaseDate); //חישוב תדירות וכמות רכישת המוצר לפי המערכים
                if (CustomerProductB.ActPurLst.Count > 0) UnusualAmountHandler(); //טיפול בחודשים בהם המוצר נרכש בכמות חריגה מהרגיל 

            }
        }


        //הפונקציה מקבלת תאריך קניה אחרון עברי ולועזי,מחזירה אוביקט מסוג דפוס התנהגות
        public void CalculateFrequency(HebrewDate_Tbl hd, DateTime lastPurchaseDate)
        {
            ///הכנסת החודשים שבהם נרכש המוצר לרשימה
            List<int> lst = MonthsInList(CustomerProductA.CounterArr);
            //בדיקה עבור החגים: ר"ה סוכות פסח שבועות
            bool isChagim = GeneralFunctions.CheckChagim(CustomerProductA.CounterArr, null, 1);
            int numOfMonths = GeneralFunctions.Months(1);
            int numOfActPurMonths = CustomerProductA.CounterArr.Count(p => p != 0 && p != -1); ;
            bool ans = CustomerProductA.CounterArr.All(p => p != 0);
            if (ans)//מוצרים חודשיים---נקנים לפחות פעם אחת בחודש
                NewBehaviourPattern(Classification.Monthly, numOfActPurMonths, numOfMonths, lastPurchaseDate, " ");
            else
            {
                //מוצר שנתי--נקנה פעם אחת בשנה 
                string name = GeneralFunctions.NameOfEvent(hd.Month, hd.Day, year - 1);
                if (numOfActPurMonths == 1 && IsPurchasedSameEvent(CustomerProductA.CustomerId, CustomerProductA.ProductId, name))
                {
                    NewBehaviourPattern(Classification.Event, numOfActPurMonths, 0, lastPurchaseDate, name);
                }
                else if (GeneralFunctions.CheckChagim(CustomerProductA.CounterArr, null, 1) && CustomerProductB.ActPurLst.Count > 0 && GeneralFunctions.CheckChagim(CustomerProductB.CounterArr, null, 2))
                    Chagim(lst);
                else
                {
                    int kind = GeneralFunctions.KindOfArr(CustomerProductA.CounterArr);//זיהוי פיזור החודשים השונים מאפס במערך
                    if (kind == frequently)
                    {
                        //המוצר נרכש במהלך השנה אך לא כל חודש
                        if ((int)Math.Round((double)numOfMonths / numOfActPurMonths) == 1)
                            NewBehaviourPattern(Classification.Monthly, numOfActPurMonths, numOfMonths, lastPurchaseDate, " ");
                        else
                            NewBehaviourPattern(Classification.Frequently, numOfActPurMonths, numOfMonths, lastPurchaseDate, " ");

                    }
                    else if (kind == summerSeason && CustomerProductB.ActPurLst.Count > 0)
                    {
                        if (GeneralFunctions.Summer_Vacation(null, CustomerProductA.DatesArr, 0) && GeneralFunctions.Summer_Vacation(null, CustomerProductB.DatesArr, 0))//המוצר נרכש בחופש הגדול?
                            NewBehaviourPattern(Classification.Periodic, numOfActPurMonths, summerVacMonthsNum, lastPurchaseDate, summerVacation);
                        else if (GeneralFunctions.FindSeason(MonthsInList(CustomerProductA.CounterArr), 1) == summer && GeneralFunctions.FindSeason(MonthsInList(CustomerProductA.CounterArr), 1) == summer)
                            NewBehaviourPattern(Classification.Periodic, numOfActPurMonths, summerMonthsNum, lastPurchaseDate, summer);

                    }
                    else if (kind == winterSeason && CustomerProductB.ActPurLst.Count > 0)
                    {  //המוצר נרכש בחודשי החורף
                        if (GeneralFunctions.FindSeason(MonthsInList(CustomerProductA.CounterArr), 1) == winter && GeneralFunctions.FindSeason(MonthsInList(CustomerProductA.CounterArr), 1) == winter)
                            NewBehaviourPattern(Classification.Periodic, numOfActPurMonths,winterMonthsNum, lastPurchaseDate, winter);

                    }
                }
            }
        }

        //טיפול בחודשים בהם המוצר נרכש בכמות חריגה מהרגיל 
        public void UnusualAmountHandler()
        {
            CalculateUsuallyAmount(CustomerProductB.AmountPerPurchaseArr, CustomerProductB.CounterArr, CustomerProductB.SumArr, CustomerProductB.ExceptionalMonthsLst);
            if (CustomerProductA.ExceptionalMonthsLst.Count >= 1 && CustomerProductB.ExceptionalMonthsLst.Count > 0)
            {
                bool isDone = true;
                bool checkChagim = GeneralFunctions.CheckChagim(null, CustomerProductA.ExceptionalMonthsLst, 1);
                if (checkChagim)
                    EventExceptionalMonths(CustomerProductA.ExceptionalMonthsLst);
                else if (CustomerProductA.ExceptionalMonthsLst.Count == 1)
                    EventExceptionalMonths(CustomerProductA.ExceptionalMonthsLst);
                else
                    isDone = PeriodExceptionalMonths(CustomerProductA.ExceptionalMonthsLst);
                if (!isDone)
                    EventExceptionalMonths(CustomerProductA.ExceptionalMonthsLst);
            }

        }


        /////////////////////////////////////////////




        ////////////////////////////////////פונקציות נוספות///////////////////////////

        //יצירת אוביקטים ובניית המערכים של מחלקת מוצר הלקוח
        public void InitCustomerProduct(int custId)
        {
            CustomerProductA = new CustomerProduct(custId);
            CustomerProductA.DatesArrBuilder();
            CustomerProductA.ZeroArrays(1);
            CustomerProductA.UpdatePurchasesHistory();
            CustomerProductB = new CustomerProduct(custId);
            CustomerProductB.DatesArrBuilder();
            CustomerProductB.ZeroArrays(2);
        }

        //איפוס מערכים
        public void ResetArrays()
        {
            CustomerProductA.ZeroArrays(1);
            CustomerProductB.ZeroArrays(2);
            CustomerProductB.DatesArrClear();
            CustomerProductA.DatesArrClear();

        }
        //איפוס רשימות
        public void ResetLists()
        {
            CustomerProductA.ExceptionalMonthsLstClear();
            CustomerProductB.ExceptionalMonthsLstClear();
            CustomerProductB.ActPurLst.Clear();
        }

        //מילוי מערכי מונה סוכם ומנה 
        public DateTime ProductPurchases(List<ActuallyPurchase_Tbl> lst)
        {
            DateTime a = CustomerProductA.ArraysFiller(lst);
            CustomerProductB.ActPurLst = dba.AllPurchasesOfProductIdOneYear
            (CustomerProductB.CustomerId, CustomerProductB.ProductId, (year - 2));
            CustomerProductB.ArraysFiller(CustomerProductB.ActPurLst);
            return a;
        }
        //הגדרת קוד מוצר
        public void SetProdId(int prodId)
        {
            CustomerProductA.ProductId = prodId;
            CustomerProductB.ProductId = prodId;
        }


        //מחזירה רשימה עם החודשים שבהם נרכש המוצר
        public List<int> MonthsInList(int[] counterArr)
        {
            List<int> lst = new List<int>();
            for (int i = 1; i < counterArr.Length; i++)
            {
                if (counterArr[i] != -1)
                    if (counterArr[i] != 0)
                        lst.Add(i);
            }
            return lst;
        }


        //הפונקציה מחזירה בחודש מסוים את התאריך הראשון בו המוצר נרכש
        public DateTime GetFirstPurchaseDateOnMonth(List<DateTime> lst)
        {
            var list = lst.OrderBy(x => x.TimeOfDay).ToList();
            DateTime dt = list.FirstOrDefault(); //מציאת תאריך קניה אחרון
            return dt;
        }
        //הפונקציה מחזירה בחודש מסוים את התאריך האחרון בו מוצר נרכש
        public DateTime GetLastPurchaseDateOnMonth(List<DateTime> lst)
        {
            var list = lst.OrderByDescending(x => x.TimeOfDay).ToList();
            DateTime dt = list.FirstOrDefault(); //מציאת תאריך קניה אחרון
            return dt;
        }
        public int MakeEqual(bool check1, bool check2, int m)
        {
            if (check1 && !check2)
            {
                if (m > 5)
                    m = m + 1;
            }
            if (!check1 && check2)
            {
                if (m > 5)
                    m = m - 1;
            }
            return m;
        }

        //פונקציה הבודקת אם מוצר כלשהו נרכש באותו זמן גם בשנה הקודמת 
        public bool IsPurchasedSameEvent(int customerId, int productId, string nameOfevent)
        {
            List<DateTime?> dates = dbph.GetLastPurchaseDatesOfProdId(productId, customerId);
            DateTime? d = dates.ElementAtOrDefault(1);//כדי לקבל את תאריך קניה אחד לפני אחרון 
            if (d == null)
                return false;
            int code = dbf.GetHebrewDateIdByForeignDate((DateTime)d);
            HebrewDate_Tbl t = dbh.GetHebrewDateByHebrewDateId(code);
            string name = GeneralFunctions.NameOfEvent(t.Month, t.Day, t.Year);
            if (nameOfevent == name)
                return true;
            return false;
        }

        ///////////////////////////////////////////////////////////////



        //////////////////////////////הכנסה בפועל דפוס התנהגות למוצר////////////////

        public void NewBehaviourPattern(Classification classification, int numOfActPurMonths, int numOfMonths, DateTime lastPurchaseDate, string nameOfevent)
        {
            int everyXmonthsBuy = (int)Math.Round((double)numOfMonths / numOfActPurMonths);
            int xTimesOnMonthsBuy = (int)Math.Round((double)(CustomerProductA.CounterArr.Where(p => p != -1).Sum()) / numOfActPurMonths);
            int amount = CalculateUsuallyAmount(CustomerProductA.AmountPerPurchaseArr, CustomerProductA.CounterArr, CustomerProductA.SumArr,
               CustomerProductA.ExceptionalMonthsLst);
            AddBehaviourPattern((int)classification, lastPurchaseDate, CustomerProductA.ProductId, nameOfevent, amount, xTimesOnMonthsBuy, everyXmonthsBuy);

        }
        //הוספת דפוס התנהגות לטבלה
        public void AddBehaviourPattern(int classification, DateTime dt, int prodId, string name, int amount, int xTimesOnMonthBuy, int everyXMonthsBuy)
        {
            BehaviourPattern_Dto newBP = new BehaviourPattern_Dto
            {
                Status = classification,
                NameOfEvent = name,
                Amount = amount,
                LastPurchaseDate = dt,
                EveryXMonthsBuy = everyXMonthsBuy,
                XTimesOnMonthBuy = xTimesOnMonthBuy,
                TimesOfAmountChange = 0,
                TimesNotFoundInPrognosis = 0,
                TimesOfCancelation = 0,
                IsNewProduct = false,
                ProductId = prodId,
                CustomerId = CustomerProductA.CustomerId,
            };

            // dbbp.AddNewBehaviourPattern(newBP.DtoTODal());
        }

        //////////////////////////////////////////////////////////


        /////טיפול בחגי השנה//////////
        public void Chagim(List<int> lst)
        {
            foreach (int month in lst)
            {

                DateTime dt = GetFirstPurchaseDateOnMonth(CustomerProductA.DatesArr[month]);
                int year = GeneralFunctions.Get_CurrentYear();
                int code = dbf.GetHebrewDateIdByForeignDate(dt);
                HebrewDate_Tbl t = dbh.GetHebrewDateByHebrewDateId(code);
                string name = GeneralFunctions.NameOfEvent(t.Month, t.Day, year - 1);
                AddBehaviourPattern((int)Classification.Event, dt, CustomerProductA.ProductId,
                name, CustomerProductA.SumArr[month], CustomerProductA.CounterArr[month], 0);
            }
        }

        //לבדוק אם בקיץ  כל החודשים שנרכשו בהם הם חריגים
        public bool Winter(List<int> lst)
        {
            List<int> lst1 = new List<int>();
            for (int i = 1; i <= winterMonthsNum; i++)
                if (CustomerProductA.CounterArr[i] != 0) lst1.Add(i);
           return !lst1.Except(lst).Any();

        }
        //לבדוק אם בחורף כל החודשים שנרכשו בהם הם חריגים
        public bool Summer(List<int> lst)
        {
            List<int> lst1 = new List<int>();
            for (int i = 7; i <= GeneralFunctions.Months(1); i++)
                if (CustomerProductA.CounterArr[i] != 0) lst1.Add(i);
            return !lst1.Except(lst).Any();

        }
        ////////////////////////בדיקות חודשים המזוהים כחריגים בכמות הנרכשה בהם///////////////////

        //טיפול בחודשים מסוג פריוד המזוהים כחודשים שנרכש בהם כמות חריגה מהרגיל     
        public bool PeriodExceptionalMonths(List<int> lst)
        {
            bool isDone = false;
            DateTime dt = new DateTime();
            int count = CustomerProductA.ExceptionalMonthsLst.Count;
            //סוכם סך הקניות שנעשו בתקופה
            int sum = 0;
            //מעבר על כל החודשים החריגים,המטרה לזהות האם חריגים בחופש הגדול, בקיץ או בחורף
            foreach (int month in lst)
            {
                dt = GetLastPurchaseDateOnMonth(CustomerProductA.DatesArr[month]);
                sum += CustomerProductA.CounterArr[month];
            }
            if (CustomerProductB.ActPurLst.Count > 0)
            {
                if (IsExceptionInSummerVacation())
                {
                    AddBehaviourPattern((int)Classification.Periodic, dt, CustomerProductA.ProductId, summerVacation, CalculateUnusualAmount(),
                        (int)Math.Round((double)(sum) / count), (int)Math.Round((double)summerVacMonthsNum / count));
                    isDone = true;
                }
                else if ( Winter(CustomerProductA.ExceptionalMonthsLst) && ExceptionInSeason() == winter)
                {
                    AddBehaviourPattern((int)Classification.Periodic, dt, CustomerProductA.ProductId, winter, CalculateUnusualAmount(),
                        (int)Math.Round((double)(sum) / count), (int)Math.Round((double)winterMonthsNum / count));
                    isDone = true;

                }
                else if (Summer(CustomerProductA.ExceptionalMonthsLst) && ExceptionInSeason() == summer)
                {
                    AddBehaviourPattern((int)Classification.Periodic, dt, CustomerProductA.ProductId, summer, CalculateUnusualAmount(),
                       (int)Math.Round((double)(sum) / count), (int)Math.Round((double)summerMonthsNum / count));
                    isDone = true;

                }
            }
            return isDone;
        }

        //בודק אם המוצר נרכש בכמות חריגה בקיץ\בחורף
        public string ExceptionInSeason()
        {
            //בדיקה אם החודשים החריגים בשנה הנוספת תחומים בעונה מסוימת?
            string anotherYearSeason = GeneralFunctions.FindSeason(CustomerProductB.ExceptionalMonthsLst, 2);
            //בדיקה אם החודשים החריגים תחומים בעונה מסוימת?
            string season = GeneralFunctions.FindSeason(CustomerProductA.ExceptionalMonthsLst, 1);
            if (season == winter && anotherYearSeason == winter)
                return winter;
            if (season == summer && anotherYearSeason == summer)
                return summer;
            return " ";
        }


        //בודק אם המוצר נרכש בכמות חריגה בחופש הגדול
        public bool IsExceptionInSummerVacation()
        {
            //בדיקה אם החודשים ברשימת החריגים תחומים בחופש הגדול?
            bool checkSummerVacation = GeneralFunctions.Summer_Vacation(CustomerProductA.ExceptionalMonthsLst, null, 0);
            //בדיקה אם החודשים החריגים בשנה הנוספת תחומים בחופש הגדול?
            bool anotherYearcheckSummerVacation = GeneralFunctions.Summer_Vacation(CustomerProductB.ExceptionalMonthsLst, null, 0);
            return checkSummerVacation && anotherYearcheckSummerVacation;
        }


        //טיפול בחודשים מסוג איונט  המזוהים כחודשים שנרכש בהם כמות חריגה מהרגיל       
        public bool EventExceptionalMonths(List<int> lst)
        {
            bool isDone = false;
            //event כל חג\מאורע מתווסף כרשומה בטבלת דפוס התנהגות תחת הסטטוס 
            foreach (int month in lst)
            {
                int code1 = dbf.GetHebrewDateIdByForeignDate(GetFirstPurchaseDateOnMonth(CustomerProductA.DatesArr[month]));
                HebrewDate_Tbl t1 = dbh.GetHebrewDateByHebrewDateId(code1);
                string name1 = GeneralFunctions.NameOfEvent(t1.Month, t1.Day, year - 1);
                int excMnth = MakeEqual(GeneralFunctions.Get_HebrewCalendar().IsLeapYear(year - 2),
                GeneralFunctions.Get_HebrewCalendar().IsLeapYear(year - 1), month);
                bool checkExceptionalAmount = CustomerProductB.ExceptionalMonthsLst.Contains(excMnth);
                int code2 = dbf.GetHebrewDateIdByForeignDate(GetFirstPurchaseDateOnMonth(CustomerProductB.DatesArr[excMnth]));
                HebrewDate_Tbl t2 = dbh.GetHebrewDateByHebrewDateId(code2);
                string name2 = GeneralFunctions.NameOfEvent(t2.Month, t2.Day, year - 2);
                //אם בשנה נוספת זוהה כמות חריגה באותו חודש
                if (!checkExceptionalAmount && name1.Equals(name2))
                    continue;
                isDone = true;
                AddBehaviourPattern((int)Classification.Event, GetLastPurchaseDateOnMonth(CustomerProductA.DatesArr[month]),
               CustomerProductA.ProductId, name1, CustomerProductA.AmountPerPurchaseArr[month], CustomerProductA.CounterArr[month], 0);
            }
            return isDone;
        }

        //////////////////////////////////////////////////////////////////




        ///////////////////////////מציאת כמות הנרכשת מהמוצר ////////////////////////
        //פונקציה המחשבת כמות הנרכשת בד"כ ומכניסה לרשימת חודשים חריגים את החודשים החריגים בכמות
        public int CalculateUsuallyAmount(int[] amountPerPurchaseArr, int[] counterArr, int[] sumArr, List<int> exceptionalMonthsLst)
        {
            //כדי למצוא כמה נקנה מהמוצר בקניה בודדת
            for (int i = 1; i <= 13; i++)
            {
                if (counterArr[i] != 0 && counterArr[i] != -1)
                    amountPerPurchaseArr[i] = (int)Math.Round((double)(sumArr[i] / counterArr[i]));
                if (counterArr[i] == -1)
                    amountPerPurchaseArr[i] = -1;
            }
            double sd = Standard_Deviation(amountPerPurchaseArr);//סטיית תקן 
            double sum = amountPerPurchaseArr.Where(p => p != -1 && p != 0).Sum();
            int count = amountPerPurchaseArr.Where(p => p != -1 && p != 0).Count();
            double avg = (double)sum / count;
            double low = (int)Math.Round((double)(avg - (sd)));
            double high = (int)Math.Round((double)(avg + (sd)));
            for (int i = 1; i <= 13; i++)
            {
                if (amountPerPurchaseArr[i] != -1 && amountPerPurchaseArr[i] != 0)
                {
                    //חודש שבו הכמות חורגת מהכמות הנורמלית
                    if (amountPerPurchaseArr[i] < low || amountPerPurchaseArr[i] > high)
                    {
                        exceptionalMonthsLst.Add(i);
                        count--;
                        sum -= amountPerPurchaseArr[i];
                    }
                }
            }
            return (int)Math.Round((double)sum / count);
        }
        //הפונקציה מחשבת ומחזירה את סטיית התקן במערך  
        public double Standard_Deviation(int[] arr)
        {
            double sum = arr.Where(p => p != -1 && p != 0).Sum();
            double avg = sum / (arr.Where(p => p != -1 && p != 0).Count());
            double sum1 = 0;
            for (int i = 1; i <= GeneralFunctions.Months(1); i++)
            {
                if (arr[i] != 0)
                {
                    double dis = Math.Pow((avg - arr[i]), 2);
                    sum1 += dis;
                }
            }
            return Math.Sqrt(sum1 / (arr.Where(p => p != -1 && p != 0).Count()));
        }

        //פונקציה המחשבת כמות לא שגרתי למוצר
        public int CalculateUnusualAmount()
        {
            int sumAmount = 0;
            int sumPurchases = 0;

            foreach (int month in CustomerProductA.ExceptionalMonthsLst)
            {
                sumPurchases += CustomerProductA.CounterArr[month];
                sumAmount += CustomerProductA.SumArr[month];
            }
            return (int)Math.Round((double)sumAmount / sumPurchases);
        }

        ////////////////////////////////////////////////
    }

}
