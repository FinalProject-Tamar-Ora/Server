using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
namespace Bll
{
   public class HebrewDateDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();
        public HebrewDate_Tbl GetHebrewDateByHebrewDateId(int id)
        {
            return db.HebrewDate_Tbl.Where(p => p.HebrewDateId == id).FirstOrDefault();
        }

      
        public int GetPartOfDate(int id)
        {
           return db.HebrewDate_Tbl.Where(p => p.HebrewDateId == id).FirstOrDefault().Day;
        }

        public int GetFirstHebrewDateIdOfYear(int hebrewYear)
        {
            return db.HebrewDate_Tbl.Where(m => m.Year == hebrewYear).OrderBy(p => p.HebrewDateId).FirstOrDefault().HebrewDateId;
        }
        public int GetLastHebrewDateIdOfYear(int hebrewYear)
        {
            return db.HebrewDate_Tbl.Where(m => m.Year == hebrewYear).OrderByDescending(p => p.HebrewDateId).FirstOrDefault().HebrewDateId;

        }
        public List<HebrewDate_Tbl> GetHbrewDatesByYear(int year)
        {
            return db.HebrewDate_Tbl.Where(p => p.Year == year).ToList();

        }
        public void InitHebrewDates(int days,int month,int year)
        {
            db.InitHebrewDateTbl(days, month, year);
        }
    }
}
