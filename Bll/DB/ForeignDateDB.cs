using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
namespace Bll
{
   public class ForeignDateDB
    {
        Shopper_DBEntities1 db = new Shopper_DBEntities1();

        public int GetHebrewDateIdByForeignDate(DateTime dt)
        {
            return db.ForeignDate_Tbl.Where(p => p.ForeignDate == dt).FirstOrDefault().HebrewDateId;

        }
        public void AddForeignDate(ForeignDate_Tbl fd)
        {
            db.ForeignDate_Tbl.Add(fd);
            db.SaveChanges();

        }

       
    }
}
