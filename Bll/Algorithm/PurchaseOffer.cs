using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
    public class PurchaseOffer
    {

        PurchasesHistoryDB dbp = new PurchasesHistoryDB();//שליפות מטבלת PurchasesHistory 
        PurchasePrognosisDB dbpg = new PurchasePrognosisDB();//שליפות מטבלת PurchasesHistory 
        BehaviourPatternDB dbbp = new BehaviourPatternDB();//שליפות מטבלת BehaviourPattern 
        ForeignDateDB dbf = new ForeignDateDB();// ForeignDate שליפות מטבלת
        HebrewDateDB dbhd = new HebrewDateDB();//שליפות מטבלת HebrewDate
        ActuallyPurchaseDB dba = new ActuallyPurchaseDB();//שליפות מטבלת ActuallyPurchase
        ProductDB dbpt = new ProductDB();
        readonly static string summerVacation = "החופש הגדול";
        public DateTime CurrentDate { get; set; }//תאריך נוכחי
        public List<PurchasePrognosis_Dto> PrognosisLst { get; set; }//רשימת מוצרים שהמערכת מציעה למשתמש
        public string SoonEvent { get; set; }//אירוע\חג קרוב
        public bool IsSummerVacation { get; set; }//האם חופש גדול?
        public string CurrentSeason { get; set; }//עונת 
        public PurchaseOffer()
        {
            CurrentDate = new DateTime();
            PrognosisLst = new List<PurchasePrognosis_Dto>();
            InitProperties();
        }
        public void InitProperties()//אתחול פרופרטיס 
        {
            int monthInNumber = GeneralFunctions.HebrewMonthInNumber(CurrentDate);//ייצוג החודש הנל במספר ,לדוג שבט =5
            int timingId = dbf.GetHebrewDateIdByForeignDate(CurrentDate);//קוד תאריך עברי של התאריך הלועזי הנוכחי
            int dayInMumber = dbhd.GetPartOfDate(timingId);//מציאת תאריך עברי ע"י קוד תאריך עברי
            SoonEvent = GeneralFunctions.NameOfEvent(monthInNumber, dayInMumber, GeneralFunctions.Get_CurrentYear());// חג\אירוע הקרוב
            IsSummerVacation = GeneralFunctions.Summer_Vacation(null, null, CurrentDate.Month);//בדיקה אם חופש הגדול
            CurrentSeason = GeneralFunctions.ConfigSeason(monthInNumber, 0);//עונה
        }

        //הפונקציה מקבלת קוד לקוח ומכניסה רכישה חדשה לטבלת היסטוריית רכישות של הלקוח
        public List<PurchasePrognosis_Dto> NewPurchase(int customerId)
        {

            PurchasesHistory_Tbl newPurchase = new PurchasesHistory_Tbl
            {
                CustomerId = customerId,
                PurchaseDate = CurrentDate,
                HebrewDateId = dbf.GetHebrewDateIdByForeignDate(CurrentDate),
                Description = "...",
                BelongToCurrentYear = true
            };

             dbp.AddNewPurchase(newPurchase);
             BuildBasketToCustomerId(customerId);
             dbpg.AddPrognosisLst(PrognosisLst);
            return PrognosisLst;
        }


        //פונקציה הבודקת אם מוצר צריך להרכש 
        private bool AddToBasket(int status, bool condition, int custId, int prodId, int xTimesOnMonth, int everyXMonthsBuy, DateTime dt)
        {
            if (condition)
            {
                if (status == (int)Classification.Event)
                    return (xTimesOnMonth > CheckXTimesOnMonth(xTimesOnMonth, custId, prodId));
                else if (status == (int)Classification.Monthly)
                    return (xTimesOnMonth > CheckXTimesOnMonth(xTimesOnMonth, custId, prodId));
                else if (status == (int)Classification.Frequently || status == (int)Classification.Periodic)
                {
                    int c = CheckXTimesOnMonth(xTimesOnMonth, custId, prodId);
                    if (c == 0 && CheckEveryXMonthsBuy(everyXMonthsBuy, dt)) return true;
                   if (c>0 && xTimesOnMonth>c) return true;


                }
            }
            return false;
        }
        //בדיקה אם מוצר צריך להרכש שוב באותו חודש
        public int CheckXTimesOnMonth(int xTimesOnMonth, int custId, int prodId)
        {
            return HowManyTimesThisMonth(custId, prodId);
            
        }
        //בדיקה כמה חודשים חלפו מאז הרכישה האחרונה,ואם המוצר צריך להרכש
        public bool CheckEveryXMonthsBuy(int everyXMonthsBuy, DateTime lastPurchaseDate)
        {
            int currYear = GeneralFunctions.Get_HebrewCalendar().GetYear(CurrentDate);
            int lastPurchaseDateYear = GeneralFunctions.Get_HebrewCalendar().GetYear(lastPurchaseDate);
            int currMonth = GeneralFunctions.Get_HebrewCalendar().GetMonth(CurrentDate);
            int lastPurchaseDateMonth = GeneralFunctions.Get_HebrewCalendar().GetMonth(lastPurchaseDate);
            int monthsApart = 12 * (currYear - lastPurchaseDateYear) + currMonth - lastPurchaseDateMonth;
            return Math.Abs(monthsApart) >= everyXMonthsBuy;
        }
        //הפונקציה מקבלת קוד לקוח ובונה ללקוח סל קניות מתאים
        public void BuildBasketToCustomerId(int currentCustomerId)
        {
            int newPurchaseId = dbp.GetNewPurchaseId(currentCustomerId);
            //הכנסה לרשימה את כל דפוסי התנהגות של המוצרים של הלקוח הנוכחי
            List<BehaviourPattern_Tbl> lstBP = dbbp.GetBehaviourPatternLstByCustId(currentCustomerId);
            var groupByProductId = lstBP.GroupBy(p => p.ProductId).ToList();//קיבוץ דפוסי ההתנהגות לפי קוד מוצר 
            //עבור על קבוצות דפוסי ההתנהגות של מוצרי הלקוח
            foreach (var group in groupByProductId)
            {
                //מיין את דפוסי התנהגות של המוצר בסדר יורד ע"פ סטטוס
                var orderByStatus = group.OrderByDescending(p => p.Status).ToList();
                //עבור על קבוצת דפוסי התנהגות של מוצר מסוים
                foreach (BehaviourPattern_Tbl b in orderByStatus)
                {
                    //תציע רק אם המוצר לא בוטל יותר מפעמיים
                    if (b.TimesOfCancelation < 2)
                    {
                        //בדיקה אם האירוע הקרוב זהה לאירוע של שם האירוע בדפוס ההתנהגות של המוצר
                        if (AddToBasket((int)Classification.Event, (b.Status == Classification.Event && b.NameOfEvent == SoonEvent)
                            , currentCustomerId, b.ProductId, (int)b.XTimesOnMonthBuy, 0, (DateTime)b.LastPurchaseDate))
                        {
                            AddPurProg(newPurchaseId, b.Amount, b.ProductId);
                            break;
                        }
                        //אם הסטטוס תקופה ושם התקופה הוא חופש גדול וגם עכשיו חופש גדול

                        else if (AddToBasket((int)Classification.Periodic, (b.Status == Classification.Periodic && b.NameOfEvent == summerVacation && IsSummerVacation),
                             currentCustomerId, b.ProductId, (int)b.XTimesOnMonthBuy, 0, (DateTime)b.LastPurchaseDate))
                        {
                            AddPurProg(newPurchaseId, b.Amount, b.ProductId);
                            break;
                        }
                        //אם הסטטוס תקופה ושם העונה של הדפוס זהה לשם העונה עבור התאריך הנוכחי
                        else if (AddToBasket((int)Classification.Periodic, (b.Status == Classification.Periodic && b.NameOfEvent == CurrentSeason),
                             currentCustomerId, b.ProductId, (int)b.XTimesOnMonthBuy, (int)b.EveryXMonthsBuy, (DateTime)b.LastPurchaseDate))
                        {
                            AddPurProg(newPurchaseId, b.Amount, b.ProductId);
                            break;
                        }
                        //אם הסטטוס לעתים קרובות
                        else if (AddToBasket((int)Classification.Frequently, (b.Status == Classification.Frequently), currentCustomerId, b.ProductId, (int)b.XTimesOnMonthBuy,
                             (int)b.EveryXMonthsBuy, (DateTime)b.LastPurchaseDate))
                        {
                            AddPurProg(newPurchaseId, b.Amount, b.ProductId);
                            break;
                        }
                        //אם הסטטוס חודשי
                        else if (AddToBasket((int)Classification.Monthly, (b.Status == Classification.Monthly), currentCustomerId, b.ProductId, (int)b.XTimesOnMonthBuy, (int)b.EveryXMonthsBuy,
                               (DateTime)b.LastPurchaseDate))
                        {
                            AddPurProg(newPurchaseId, b.Amount, b.ProductId);
                            break;
                        }


                    }
                }
            }

        }

        //הוספת מוצר לסל הקניות
        private void AddPurProg(int newPurchaseId, int? amount, int productId)
        {
            PrognosisLst.Add(new PurchasePrognosis_Dto
            {
                PurchasesHistoryId = newPurchaseId,
                Amount = amount,
                ProductId = productId,
                Name = dbpt.GetNameOfProductByProdId(productId),
                Description = dbpt.GetDescriptionOfProductByProdId(productId),
                Image = dbpt.GetImageOfProductByProdId(productId)
            });

        }





        //הפונקציה מקבלת קוד לקוח נוכחי וקוד מוצר ומחזירה את מספר הפעמים שהמוצר נרכש בחודש הנוכחי
        public int HowManyTimesThisMonth(int custId, int productId)
        {
            int counter = 0;
            ///חפש בקניות השייכות לשנה זו וגם נקנו בחודש העכשוי
            List<PurchasesHistory_Tbl> listPH = dbp.GetPurchasesByCustIdAndCurrDate(custId, CurrentDate, dbp.GetNewPurchaseId(custId)).ToList();
            foreach (PurchasesHistory_Tbl ph in listPH)
            {
                List<ActuallyPurchase_Tbl> listAP = dba.GetActuallyPurchasesByPurchaseHistoryId(ph.PurchasesHistoryId).ToList();
                if (listAP.Exists(p => p.ProductId == productId))
                    counter++;
            }
            return counter;
        }
    }
}
