using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;


namespace VikaProject
{
    class WorkingWithDB
    {
        private SQLiteConnection db;
        SQLiteDataReader reader;
        SQLiteCommand command;
        public WorkingWithDB()
        {
            db = new SQLiteConnection("Data Source = MyDB.db;");
            db.Open();

        }
        public void Add(CalendarEvent calendarEvent)
        {
            command = new SQLiteCommand("insert into Color(A,B,G,color_id) values (@A,@B,@G,@color_id)", db);
            command.Parameters.AddWithValue("@A", calendarEvent.currentcolor.A);
            command.Parameters.AddWithValue("@B", calendarEvent.currentcolor.B);
            command.Parameters.AddWithValue("@G", calendarEvent.currentcolor.G);
            command.Parameters.AddWithValue("@color_id", null);

            command.ExecuteNonQuery();
            int id = GetCurrentColor_id(calendarEvent);
            command = new SQLiteCommand("insert into CalendarEvent(Event_id,event_time,deadline,description,color_id) values " +
                                        "(@Event_id,@event_time,@deadline,@description,@color_id)", db);
            command.Parameters.AddWithValue("@Event_id", null);
            command.Parameters.AddWithValue("@event_time", calendarEvent.event_time.ToShortDateString());
            command.Parameters.AddWithValue("@deadline", calendarEvent.deadline.ToShortDateString());
            command.Parameters.AddWithValue("@description", calendarEvent.description);
            command.Parameters.AddWithValue("@color_id", id);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        int GetCurrentColor_id(CalendarEvent calendarEvent)
        {
            command = new($"SELECT color_id from Color " +
                                                   $"where A={calendarEvent.currentcolor.A}" +
                                                   $" and B={calendarEvent.currentcolor.B}" +
                                                   $" and G={calendarEvent.currentcolor.G}", db);

            reader = command.ExecuteReader();
            foreach (DbDataRecord el in reader)
            {
                return int.Parse(el["color_id"].ToString());
            }

            return 0;
        }
    }
}
