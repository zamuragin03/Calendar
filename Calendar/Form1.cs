using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calendar__;

namespace Calendar__
{
    
    public partial class Form1 : Form
    {
        DateTime now;
        public Form1()
        {
            InitializeComponent();

            string[] DaysOfWeek = { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            now = DateTime.Now;
            current_month.Text = $"{now.ToString("MMMM")}";
            for (int i = 0; i < 7; i++)
            {
                panel.Controls.Add(new Label() { 
                Text= DaysOfWeek[i]},
                i,1
                );
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            now =now.AddMonths(-1);
            current_month.Text = $"{now.ToString("MMMM")}";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            now = now.AddMonths(1);
            current_month.Text = $"{now.ToString("MMMM")}";
        }
    }
}
