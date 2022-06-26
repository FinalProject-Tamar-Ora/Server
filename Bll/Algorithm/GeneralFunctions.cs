using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
namespace Bll
{
    public static class GeneralFunctions
    {
        // static Shopper_DBEntities1 db = new Shopper_DBEntities1();
        static ForeignDateDB dbf = new ForeignDateDB();
        static HebrewDateDB dbh = new HebrewDateDB();
        static HebrewCalendar hc = new HebrewCalendar();
        static int currentYear = 5782;
        public static HebrewCalendar Get_HebrewCalendar()
        {
            return hc;
        }
        public static int Get_CurrentYear()
        {
            return currentYear;
        }
        public static int Months(int num)
        {

            if (hc.IsLeapYear(currentYear - num))
                return 13;
            return 12;
        }
        public static string ConfigSeason(int month, int num)
        {
           
            if (hc.IsLeapYear(currentYear - num))
            {
                if ((month >= 8 && month <= 13) || month == 1)
                    return "קיץ";
                if (month >= 2 && month <= 7)
                    return "חורף";
            }
            else
            {
                if ((month >= 7 && month <= 12) || month == 1)
                    return "קיץ";
                if (month >= 2 && month <= 6)
                    return "חורף";
            }

            return " ";

        }
        public static int HebrewMonthInNumber(DateTime dt)
        {
           return hc.GetMonth(dt);
        }
        public static string FindSeason(List<int> lst, int num)
        {
            if (lst != null && lst.Count == 0)
                return " ";
            bool isSummer = true, isWinter = true;
            foreach (int month in lst)
            {
                if (ConfigSeason(month, num) != "קיץ")
                {
                    isSummer = false;
                    break;
                }
            }
            if (isSummer)
                return "קיץ";
            foreach (int month in lst)
            {
                if (ConfigSeason(month, num) != "חורף")
                {
                    isWinter = false;
                    break;
                }
            }
            if (isWinter)
                return "חורף";
            return " ";
        }
        public static bool Summer_Vacation(List<int> lstExceptional, List<DateTime>[] lstDates, int month)
        {
            if (month != 0)
            {
                if (month >= 7 && month <= 8)
                    return true;
                return false;
            }
            if (lstExceptional != null && lstExceptional.Count > 0)
            {
                foreach (int i in lstExceptional)
                    if (i < 7 || i > 8)
                        return false;
                return true;
            }
            if (lstDates != null)
            {
                for (int i = 0; i < lstDates.Length; i++)
                {
                    foreach (DateTime date in lstDates[i])
                        if (date.Month < 7 || date.Month > 8)
                            return false;
                }
                return true;
            }

            return false;
        }

        //הפונקציה מקבלת מערך עם תאריכים ומחזירה ערך בוליאני אם כל התאריכים שייכים לאותה עונה
        public static bool SameSeasons(DateTime[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int id1 = dbf.GetHebrewDateIdByForeignDate(arr[i]);
                HebrewDate_Tbl ti = dbh.GetHebrewDateByHebrewDateId(id1);
                string seas1 = ConfigSeason(ti.Month, 1);
                for (int j = i; j < arr.Length; j++)
                {
                    int id2 = dbf.GetHebrewDateIdByForeignDate(arr[j]);
                    HebrewDate_Tbl tj = dbh.GetHebrewDateByHebrewDateId(id2);
                    string seas2 = ConfigSeason(tj.Month, 1);
                    if (seas1 != seas2)
                        return false;

                }
            }
            return true;

        }
        public static bool CheckChagim(int[] arr, List<int> opt, int num)
        {
            if (opt != null)
            {
                if (hc.IsLeapYear(currentYear - num))
                {
                    foreach (int i in opt)//שנה מעוברת
                        if (i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 9 || i == 11 || i == 12)
                            return false;
                }
                else
                {
                    foreach (int i in opt)
                        if (i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 8 || i == 10 || i == 11)
                            return false;
                }

            }
            bool isChagim = true;
            if (arr != null)
            {
                if (hc.IsLeapYear(currentYear - num))
                {
                    for (int i = 1; i < arr.Length && isChagim; i++)
                    {

                        //חודשים שלא קשורים לחגים
                        if (i == 2 && arr[i] != 0)
                            isChagim = false;
                        if (i == 3 && arr[i] != 0)
                            isChagim = false;
                        if (i == 4 && arr[i] != 0)
                            isChagim = false;
                        if (i == 5 && arr[i] != 0)
                            isChagim = false;
                        if (i == 6 && arr[i] != 0)
                            isChagim = false;
                        if (i == 7 && arr[i] != 0)
                            isChagim = false;
                        if (i == 9 && arr[i] != 0)
                            isChagim = false;
                        if (i == 11 && arr[i] != 0)
                            isChagim = false;
                        if (i == 12 && arr[i] != 0)
                            isChagim = false;

                        //כל החודשים הנותרים-חודשי החגים

                    }
                    return isChagim;
                }
                for (int i = 1; i < arr.Length && isChagim; i++)
                {
                    //חודשים שלא קשורים לחגים
                    if (i == 2 && arr[i] != 0)
                        isChagim = false;
                    if (i == 3 && arr[i] != 0)
                        isChagim = false;
                    if (i == 4 && arr[i] != 0)
                        isChagim = false;
                    if (i == 5 && arr[i] != 0)
                        isChagim = false;
                    if (i == 6 && arr[i] != 0)
                        isChagim = false;
                    if (i == 9 && arr[9] == 0)
                        isChagim = false;
                    if (i == 8 && arr[i] != 0)
                        isChagim = false;
                    if (i == 10 && arr[i] != 0)
                        isChagim = false;
                    if (i == 11 && arr[i] != 0)
                        isChagim = false;

                    //כל החודשים הנותרים-חודשי החגים

                }

            }
            return isChagim;
        }
        public static string NameOfEvent(int month, int day, int year)
        {
            if (hc.IsLeapYear(year))
            {
                switch (month)
                {
                    case 1:
                        if (day <= 22) return "סוכות";
                        return "תשרי";
                    case 2: return "חשוון";
                    case 3: return "חנוכה";
                    case 4: return "טבת";
                    case 5:
                        if (day <= 15) return "טו שבט";
                        return "שבט";
                    case 6: return "אדר";
                    case 7:
                        if (day <= 15) return "פורים";
                        return "אדר";
                    case 8:
                        if (day <= 22) return "פסח";
                        return "ניסן";
                    case 9:
                        if (day <= 5) return "יום העצמאות";
                        return "לג בעומר";
                    case 10:
                        if (day <= 5)
                            return "שבועות";
                        return "סיון";
                    case 11: return "תמוז";
                    case 12: return "אב";

                    case 13: return "ראש השנה";
                    default: return "";

                }
            }
            else
            {
                switch (month)
                {
                    case 1:
                        if (day <= 24) return "סוכות";
                        return "תשרי";
                    case 2: return "חשוון";
                    case 3: return "חנוכה";
                    case 4: return "טבת";
                    case 5:
                        if (day <= 15) return "טו שבט";
                        return "שבט";
                    case 6:
                        if (day <= 15) return "פורים";
                        return "אדר";
                    case 7:
                        if (day <= 22) return "פסח";
                        return "ניסן";
                    case 8:
                        if (day <= 5) return "יום העצמאות";
                        if (day <= 18)
                            return "לג בעומר";
                        return "אייר";
                    case 9:
                        if (day <= 5)
                            return "שבועות";
                        return "סיון";
                    case 10: return "תמוז";
                    case 11: return "אב";

                    case 12: return "ראש השנה";
                    default: return "";

                }
            }


        }
        //פונקציה מקבלת תאריך לועזי ומחזירה את החודש העברי

        public static RequestResult GetHebrewJewishDateString(DateTime anyDate)
        {
            System.Text.StringBuilder hebrewFormatedString = new System.Text.StringBuilder();
            // Create the hebrew culture to use hebrew (Jewish) calendar
            CultureInfo jewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
            jewishCulture.DateTimeFormat.Calendar = new HebrewCalendar();

            // Day of the week in the format " "
            hebrewFormatedString.Append(anyDate.ToString("dddd", jewishCulture) + " ");
            // Day of the month in the format "'"
            hebrewFormatedString.Append(anyDate.ToString("dd", jewishCulture) + " ");
            // Month and year in the format " "
            hebrewFormatedString.Append("" + anyDate.ToString("y", jewishCulture));
            return new RequestResult() { Data = hebrewFormatedString.ToString(), Message = "success", Status = true };

            // return hebrewFormatedString.ToString();
        }

        //חקירה על אופי מערך קאונט 
        public static int KindOfArr(int[] counterArr)
        {
            bool check = Get_HebrewCalendar().IsLeapYear(Get_CurrentYear() - 1);
            int a = 0, b = 0, c = 0, d = 0;
            //מעבר על רבע ראשון של המערך  
            for (int i = 1; i <= 3; i++)
            {
                if (counterArr[i] != 0)
                    a++;
            }
            if (check)
            {
                //מעבר על רבע שני של המערך
                for (int i = 4; i <= 7; i++)
                {
                    if (counterArr[i] != 0)
                        b++;
                }
            }
            else
            {
                //מעבר על רבע שני של המערך
                for (int i = 4; i <= 6; i++)
                {
                    if (counterArr[i] != 0)
                        b++;
                }
            }
            if (check)
            {
                //מעבר על רבע שלישי של המערך
                for (int i = 8; i <= 10; i++)
                {
                    if (counterArr[i] != 0)
                        c++;
                }
            }
            else
            {
                //מעבר על רבע שלישי של המערך
                for (int i = 7; i <= 9; i++)
                {
                    if (counterArr[i] != 0)
                        c++;
                }
            }
            if (check)
            {
                //מעבר על רבע רביעי של המערך
                for (int i = 11; i <= 13; i++)
                {
                    if (counterArr[i] != 0)
                        d++;
                }
            }
            else
            {
                //מעבר על רבע רביעי של המערך
                for (int i = 10; i <= 12; i++)
                {
                    if (counterArr[i] != 0)
                        d++;
                }
            }

            //פרישה  במהלך השנה
            if ((a != 0 && b != 0 && c != 0 && d != 0) || (a != 0 && b != 0 && c != 0 && d == 0) || (a != 0 && b != 0 && c == 0 && d != 0)
                || (a != 0 && b == 0 && c != 0 && d != 0)
                || (a == 0 && b != 0 && c != 0 && d != 0))
                return 1;
            if ((a != 0 && b == 0 && c != 0 && d == 0) || (a == 0 && b != 0 && c != 0 && d == 0) || (a == 0 && b != 0 && c == 0 && d != 0))
                return 1;
            //זיהוי קיץ
            if (d != 0 && c != 0 && b == 0 && a >= 0)
                return 2;
            //זיהוי חורף
            if (a >= 0 && b != 0 && d == 0 && c == 0)
                return 3;
            return 0;
        }
    }
}
