using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VikaProject
{
    public partial class Form1 : Form
    {
        private DateTime now;
        readonly string[] DaysOfWeek;

        private DateTime selectedDateTime;
        private Color selected_color;
        WorkingWithDB Db;
        List<CalendarEvent> dayEvents;
        private List<int> days;

        private int selected_event_id = 0;
        public Form1()
        {
            InitializeComponent();
            DaysOfWeek = new[] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            now = DateTime.Now;
            current_month.Text = $"{now.ToString("MMMM")}";
            CurrentDate.Text = now.ToShortDateString();
            Db = new();
            Helper.get(now.Year, now.Month);
            days = Helper.days;
            selected_color = Color.AliceBlue;
            LoadCaledar();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            now = now.AddMonths(1);
            current_month.Text = now.ToString("MMMM");
            current_year.Text = now.Year.ToString();
            LoadCaledar();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            now = now.AddMonths(-1);
            current_month.Text = now.ToString("MMMM");
            current_year.Text = now.Year.ToString();
            LoadCaledar();

        }

        void LoadCaledar()
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
            for (int i = 0; i < 36; i++)
            {
                int column = i % 7;
                int row = i / 7;


                if (days[i] != 0)
                {

                    Label lbl= new Label()
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
            UpdateCurrentPlans();
        }


        void UpdateCurrentPlans()
        {
            dayEvents = Db.GetAllEvents();

            foreach (var label in panel.Controls)
            {
                try
                {

                    if (label is Label lbl)
                    {

                        foreach (var day in dayEvents)
                        {
                            if (DateTime.Parse(lbl.Text + "." + now.Month + "." + now.Year)==day.event_time.Date)
                            {
                                lbl.BackColor = Color.FromArgb(day.currentcolor.R, day.currentcolor.G,
                                    day.currentcolor.B);

                            }
                        }
                    }
                }
                catch (Exception e)
                {

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
            current_label.BackColor = Color.LightSkyBlue;

            selectedDateTime = DateTime.Parse($"{current_label.Text}.{now.Month}.{now.Year}");
            CurrentDate.Text = selectedDateTime.ToShortDateString();

            UpdateCurrentPlans();
            UpdateEventPanel();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorpicker.ShowDialog()== DialogResult.OK)
            {
                selected_color = colorpicker.Color;
                colorpanel.BackColor = selected_color;
            }
        }

        private void UpdateEventPanel()
        {

            panel_event.Controls.Clear();

            panel_event.RowCount = 1;
            panel_event.ColumnCount = 2;

            //panel_event.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,80F));
            //panel_event.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,20F));




            foreach (var el in (Db.GetAllEvents()).Where(x => x.event_time == selectedDateTime))
            {
                //events_list.AppendText(el + " \n");


                Label lbl = new Label()
                {
                    Text = el.ToString(),
                    AutoSize = true,
                    Font = new Font("Times New Roman", 16, FontStyle.Bold),
                    Margin = new Padding(6),

                };
                lbl.Click += SetCurrentEvent;

                Panel p = new()
                {
                    BackColor = Color.FromArgb(el.currentcolor.R, el.currentcolor.G, el.currentcolor.B),
                    Size = new Size(panel_event.GetColumnWidths()[0]-10, 20),
                    Margin = new Padding(6)
                };

                panel_event.Controls.Add(lbl, 0, -1);
                panel_event.Controls.Add(p, -1, 0);

            }

        }

        private void SetCurrentEvent(object sender, EventArgs e)
        {
            foreach (var el in panel_event.Controls)
            {
                if (el is Label lb)
                {
                   lb.BackColor = Color.White;
                }
            }
            Label current_event = sender as Label;
            current_event.BackColor = Color.OrangeRed;
            string aaa = current_event.Text;
            int pos = aaa.LastIndexOf(')');
            string bbb = aaa.Substring(pos);
            int id = int.Parse(aaa.Substring(0, pos));
            selected_event_id = id;
        }

        private void Add_event(object sender, EventArgs e)
        {
            if (DescriptionBox.Text.Trim()=="")
            {
                MessageBox.Show("Заполните описание");
                return;
            }

            CalendarEvent eEvent = new CalendarEvent()
            {
                event_time = DateTime.Parse(CurrentDate.Text),
                deadline = dateTimePicker1.Value.Date,
                description = DescriptionBox.Text,
                currentcolor = selected_color,
            };

            Db.Add(eEvent);
            UpdateCurrentPlans();
            UpdateEventPanel();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Подтвердите действие", "Помощь", MessageBoxButtons.YesNo);

            switch (dialogResult)
            {
                case DialogResult.Yes:
                    Db.ClearDB();
                    break;
                case DialogResult.No:
                    return;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (selected_event_id==0)
            {
                MessageBox.Show("событие не выбрано");
                return;
            }
            
            EditEvent form = new EditEvent(selected_event_id,Db);
            form.Show();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (selected_event_id ==0)
            {
                MessageBox.Show("событие не выбрано");
                return;
            }
            Db.DeleteEventById(selected_event_id);
            selected_event_id = 0;
            UpdateEventPanel();
        }
    }
}
