using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll
{
    class InitForeignTbl
    {
        public Calendar HebCal { get; set; }
       


        public  void Init()
        {
            HebCal = new HebrewCalendar();
            int curMonth = HebCal.GetMonth(DateTime.Today);
            Console.WriteLine(curMonth);
        }

       
        

    }
}
