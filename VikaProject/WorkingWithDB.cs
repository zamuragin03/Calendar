using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Security.Cryptography;


namespace VikaProject
{
    public class WorkingWithDB
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
            command = new SQLiteCommand("insert into Color(R,G,B,color_id) values (@R,@G,@B,@color_id)", db);
            command.Parameters.AddWithValue("@color_id", null);
            command.Parameters.AddWithValue("@R", calendarEvent.currentcolor.R);
            command.Parameters.AddWithValue("@G", calendarEvent.currentcolor.G);
            command.Parameters.AddWithValue("@B", calendarEvent.currentcolor.B);

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
                                                   $"where R={calendarEvent.currentcolor.R}" +
                                                   $" and G={calendarEvent.currentcolor.G}" +
                                                   $" and B={calendarEvent.currentcolor.B}", db);
            int id=0;
            reader = command.ExecuteReader();
            foreach (DbDataRecord el in reader)
            {
                id=int.Parse(el["color_id"].ToString());
            }
            

            return id;
        }

        public List<CalendarEvent> GetAllEvents()
        {
            command = new($"SELECT Event_id, event_time,deadline,description, R,G,B from CalendarEvent c " +
                          $"inner join Color clr on c.color_id = clr.color_id", db);
            List<CalendarEvent> temp = new();
            reader = command.ExecuteReader();
            foreach (DbDataRecord el in reader)
            {
               temp.Add(new CalendarEvent()
               {
                   Event_id = int.Parse(el["Event_id"].ToString()),
                   currentcolor =Color.FromArgb(int.Parse(el["R"].ToString()),int.Parse(el["G"].ToString()), int.Parse(el["B"].ToString())),
                   deadline = DateTime.Parse(el["deadline"].ToString()),
                   description = el["description"].ToString(),
                   event_time = DateTime.Parse(el["event_time"].ToString()),
               });
            }

            return temp;
        }

        public void ClearDB()
        {
            command = new SQLiteCommand("delete from CalendarEvent", db);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("delete from Color", db);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='CalendarEvent';" +
                                        "UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Color';", db);
            command.ExecuteNonQuery();
        }


        public void DeleteEventById(int EventId)
        {
            command = new SQLiteCommand($"delete from CalendarEvent where Event_id={EventId}", db);
            command.ExecuteNonQuery();
        }

        public CalendarEvent GetEventByIndex(int EventId)
        {
            command = new($"SELECT Event_id, event_time,deadline,description, R,G,B from CalendarEvent c " +
                          $"inner join Color clr on c.color_id = clr.color_id " +
                          $"where Event_id={EventId}", db);
            List<CalendarEvent> temp = new();
            reader = command.ExecuteReader();
            foreach (DbDataRecord el in reader)
            {
                return new CalendarEvent()
                {
                    Event_id = int.Parse(el["Event_id"].ToString()),
                    currentcolor = Color.FromArgb(int.Parse(el["R"].ToString()), int.Parse(el["G"].ToString()), 
                        int.Parse(el["B"].ToString())),
                    deadline = DateTime.Parse(el["deadline"].ToString()),
                    description = el["description"].ToString(),
                    event_time = DateTime.Parse(el["event_time"].ToString()),
                };
            }

            return new CalendarEvent();
        }

        public void EditEventByEventId(int EventId,CalendarEvent eEvent)
        {
            command = new($"Update CalendarEvent " +
                          $"set event_time= '{eEvent.event_time.ToShortDateString()}', " +
                          $"deadline= '{eEvent.deadline.ToShortDateString()}', " +
                          $"description= '{eEvent.description}' " +
                          $"where Event_id={EventId} ", db);


            command.ExecuteNonQuery();
            command = new($"select Color.color_id from CalendarEvent " +
                          $"inner join Color on Color.color_id = CalendarEvent.color_id " +
                          $"where Event_id = {EventId}", db);

            command = new($"Update Color " +
                          $"set R= {eEvent.currentcolor.R}, " +
                          $"G= {eEvent.currentcolor.G}, " +
                          $"B= {eEvent.currentcolor.B} " +
                          $"where color_id={EventId}", db);
            command.ExecuteNonQuery();

        }

        public List<(DateTime, int)> GetStat(int month)
        {
            command = new($"select event_time, count(event_time) from CalendarEvent " +
                          $"where  event_time like '__.{month}.%' " +
                          $"group by event_time", db);
            List<(DateTime, int)> temp = new();
            reader = command.ExecuteReader();
            foreach (DbDataRecord el in reader)
            {
                temp.Add((DateTime.Parse(el["event_time"].ToString()), int.Parse(el["count(event_time)"].ToString())));
            }

            return temp;
        }
    }
}
