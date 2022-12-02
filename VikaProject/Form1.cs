using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VikaProject
{
    public partial class Form1 : Form
    {
        DateTime now;
        string[] DaysOfWeek;

        private DateTime selectedDateTime;
        private Label prevselectedLabel;
        private Color selected_color;
        public Form1()
        {
            InitializeComponent();
            DaysOfWeek = new[] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            now = DateTime.Now;
            current_month.Text = $"{now.ToString("MMMM")}";
            
            UpdateCaledar();
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            now = now.AddMonths(1);
            current_month.Text = $"{now.ToString("MMMM")}";
            current_year.Text = now.Year.ToString();
            UpdateCaledar();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            now = now.AddMonths(-1);
            current_month.Text = $"{now.ToString("MMMM")}";
            current_year.Text = now.Year.ToString();
            UpdateCaledar();

        }

        void UpdateCaledar()
        {
            panel.Controls.Clear();
            for (int i = 0; i < 7; i++)
            {
                panel.Controls.Add(new Label()
                {
                    Text = DaysOfWeek[i],
                    Font = new Font("Adobe Gothic Std", 15, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopCenter,
                    AutoSize = true
                },
                i, 0
                );
            }
            Helper.get(now.Year, now.Month);
            List<int> days = Helper.days;

            for (int i = 0; i < 36; i++)
            {
                int column = i % 7;
                int row = i / 7;
                if (days[i] != 0)
                {
                    Label lbl = new Label()
                    {
                        Text = days[i].ToString(),
                        Font = new Font("Adobe Gothic Std", 25, FontStyle.Regular),
                        TextAlign = ContentAlignment.TopCenter,
                        AutoSize = true
                    };
                    lbl.Click += GetInfo;
                    panel.Controls.Add(lbl,row: row + 1, column: column);
                }

            }

        }

        private void GetInfo(object sender, EventArgs e)
        {

            foreach (Label label in panel.Controls)
            {
                label.BackColor=Color.AliceBlue;
            }
            Label current_label = sender as Label;
            current_label.BackColor=Color.DeepSkyBlue;
            //current_label.BackColor=Color.FromArgb();


            //current_label.BackColor = Color.Brown;

            //if (!current_label.Equals(prevselectedLabel))
            //{
            //    prevselectedLabel = null;
            //    current_label.BackColor=Color.AliceBlue;
            //}
            //else
            //{
            //    prevselectedLabel = current_label;
            //}
            selectedDateTime = DateTime.Parse($"{current_label.Text}.{now.Month}.{now.Year}");
            CurrentDate.Text = selectedDateTime.ToShortDateString();
            //MessageBox.Show(el.Text+"."+now.Month+"."+now.Year);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorpicker.ShowDialog()== DialogResult.OK)
            {
                selected_color = colorpicker.Color;
                button3.BackColor = selected_color;
            }
        }
    }
}
