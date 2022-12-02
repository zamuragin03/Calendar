using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public enum Months
    {
        Январь = 1,
        Февраль = 2,
        Март = 3,
        Апрель = 4,
        Май = 5,
        Июнь = 6,
        Июль = 7,
        Август = 8,
        Сентябрь = 9,
        Октябрь = 10,
        Ноябрь = 11,
        Декабрь = 12
    }
    public static class GetMOnth
    {
        public static List<int> days;
        public static void get(int yearnumber, int monthnumber)
        {

            days = new List<int>();

            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            DateTime myDT = new DateTime(yearnumber, 1, 1, myCal);
            myDT = myDT.AddMonths(monthnumber - 1);
            int month2 = myDT.Month;
            Console.WriteLine((Months)month2);
            int month = myCal.GetMonth(myDT);
            int j = Convert.ToInt32(myCal.GetDayOfWeek(myDT)) - 1;

            for (int m = 0; m < j; m++)
            {
                days.Add(0);
            }

            while (myCal.GetMonth(myDT) == month)
            {
                if (j % 7 == 0)
                {
                    Console.WriteLine();
                }
                days.Add(myDT.Day);
                Console.Write(myDT.Day + " ");
                myDT = myDT.AddDays(1);
                j++;
            }



        }
    }
}
