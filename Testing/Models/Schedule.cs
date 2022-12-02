//========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// Schedule.cs
// Declares a series of variables to read
// from the database
// Part of the models component of MVC


using System.Collections.Generic;

namespace SSR.Models
{
    public class Schedule
    {

        public Schedule()
        {
        }
        // A unique identifier:
        public int ScheduleID { get; set; }
        // Program name + URL; don't forget to parse
        public string ProgramName { get; set; }
        public string URL { get; set; }
        // The day, represented as a binary #:
        public int Day { get; set; }

        // Start hour and minute:
        public int StartTimeHour { get; set; }
        public int StartTimeMin { get; set; }
        // End hour and minute:
        public int EndTimeHour { get; set; }
        public int EndTimeMin { get; set; }
        // Repeating? If not, we can delete:
        public bool Repeating { get; set; }
        // If true, it is shoutcast; if false, it is Icecast:
        public bool Shoutcast { get; set; }
        // GenreID, which is admittedly superfluous:
        public int GenreID { get; set; }
        // We need to enumerate the genres:
        // 
        public IEnumerable<Genre> Genres { get; set; }
        // Also Recstatus:
        public IEnumerable<Recstatus> Recstatus { get; set; }
        // The days of the week is represented as a single
        // variable, but we will represent them here as checkboxes
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        // Finally, a checkbox to indicate PM:
        public bool StartTimePM { get; set; }
        public bool EndTimePM { get; set; }
        //We will represent them in the table as a single string:
        public string daystr { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        //The current status of the database (queued, recording, etc.):
        public string Status { get; set; }
        // And the password:
        public string Password { get; set; }
        // This is a time/date that gets updated as long as SSR is running:
        public string lastmod { get; set; }

    }


}
