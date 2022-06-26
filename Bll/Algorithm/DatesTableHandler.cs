using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;


namespace Bll
{
    public class DatesTableHandler
    {
      
        HebrewDateDB dbhd = new HebrewDateDB();
        ForeignDateDB dbf = new ForeignDateDB();
        //פונקציה מקבלת חודש ומספר ימים בחודש בשנה מסוימת ומכניסה לטבלת תאריכים עבריים
        public void InitHebrewDates(int days, int month, int year)
        {
            dbhd.InitHebrewDates(days, month, year);
        }
        //פונקציה מקבלת שנה עברית וממירה את תאריכי השנה הזאת לתאריכים לועזיים ומכניסה לטבלת תאריכים לועזיים
        public void InitForeignDates(int year)
        {
            Calendar hCal = new HebrewCalendar();
            List<HebrewDate_Tbl> l = dbhd.GetHbrewDatesByYear(year);
            DateTime dtAnyDateHebrew;
            
            foreach (HebrewDate_Tbl item in l)
            {

                dtAnyDateHebrew = hCal.ToDateTime(year, item.Month, item.Day, 0, 0, 0, 0);
                    ForeignDate_Tbl t = new ForeignDate_Tbl { HebrewDateId = item.HebrewDateId, ForeignDate = dtAnyDateHebrew };
                      dbf.AddForeignDate(t);
            }
        }
    }
}
