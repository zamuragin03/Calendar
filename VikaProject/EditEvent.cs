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
    public partial class EditEvent : Form
    {
        private int EventId;
        private WorkingWithDB Db;
        
        public EditEvent(int EventId, WorkingWithDB Db)
        {
            InitializeComponent();
            this.EventId = EventId;
            this.Db = Db;
            CalendarEvent eEvent = Db.GetEventByIndex(EventId);
            CurrentDate.Text = eEvent.event_time.ToShortDateString();
            dateTimePicker1.Text = eEvent.deadline.ToShortDateString();
            DescriptionBox.Text = eEvent.description;
            colorpanel.BackColor= eEvent.currentcolor;
        }

        private void Edit_Event(object sender, EventArgs e)
        {
            CalendarEvent eEvent = new CalendarEvent()
            {
                event_time = DateTime.Parse(CurrentDate.Text),
                deadline = dateTimePicker1.Value.Date,
                description = DescriptionBox.Text,
                currentcolor = colorpanel.BackColor,
            };
            Db.EditEventByEventId(EventId, eEvent);
            Hide();
        }

        private void colorpanel_Click(object sender, EventArgs e)
        {
            if (colorpicker.ShowDialog() == DialogResult.OK)
            {
                colorpanel.BackColor = colorpicker.Color;
            }
        }
    }
}
