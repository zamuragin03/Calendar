using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VikaProject
{
    public partial class StatForm : Form
    {
        public StatForm(List<(DateTime,int)> stat)
        {
            InitializeComponent();
            chart1.Titles.Add("Статитика планов на месяц");
            chart1.Series["s1"].IsValueShownAsLabel = true;

            for (int i = 0; i < 31; i++)
            {
                foreach (var el in stat)
                {
                    if (el.Item1.Day == i )
                    {
                        chart1.Series["s1"].Points.AddXY(i, el.Item2);
                    }
                }
               
            }
           

        }
    }
}
