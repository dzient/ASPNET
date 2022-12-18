//===========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
// ScheduleRepository.cs
// This retrieves data from the Schedule
// table
//--------------------------------------------

using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace SSR.Models
{
    public class ScheduleRepository : IScheduleRepository
    {
        // The database connection:
        private readonly IDbConnection conn;
        /// <summary>
        /// private object _conn;
        /// </summary>
        /// <param name="_conn"></param>

        //---------------------------------------------
        // ScheduleRepository
        // Constructor for ScheduleRepository
        // 
        // PARAMS : _conn (IDbConnection object)
        // RETURNS: Nothing; conn is initialized
        // 
        //----------------------------------------------
        public ScheduleRepository(IDbConnection _conn)
        {
            conn = _conn;
        }
        //---------------------------------------------
        // GetAllPrograms
        // Get an IEnumerable-derived list of programs
        //
        // PARAMS: Nothing
        // RETURNS: IEnumerbale-dervied list of programs
        // 
        //----------------------------------------------
        public IEnumerable<Schedule> GetAllPrograms()
        {
            return conn.Query<Schedule>("SELECT * FROM SCHEDULE;");

        }
        //----------------------------------------------
        // GetBoolVal
        // Return true if integer equals 1; false otherwise
        //
        // PARAMS: Integer (n)
        // RETURNS: boolean value indicated if int equals 1
        //
        //-------------------------------------------------

        public bool GetBoolVal(int n)
        {
            return (n == 1);
        }
        //----------------------------------------------
        // GetIntVal
        // Return 1 if n is true; 0 otherwise
        //
        // PARAMS: bool (n)
        // RETURNS: 1 if n is true; 0 otherwise
        //
        //-----------------------------------------------

        public int GetIntVal(bool n)
        {
            if (n) return 1;
            return 0;
        }
        //----------------------------------------------
        // Get Program
        // Given a ScheduleID, return the record for the
        // program
        // PARAMS: Integer (id = ScheduleID)
        // RETURNS: Record representing a program
        // Also fills out checkboxes representing which
        // days were selected; also check the PM checkbox
        // and subtract 12 when appropriate
        //
        //-----------------------------------------------
        public Schedule GetProgram(int id)
        {
            Schedule retval = conn.QuerySingle<Schedule>("SELECT * FROM SCHEDULE WHERE SCHEDULEID = @id",
                new { id = id });

            retval.Sunday = GetBoolVal(retval.Day & 0x1);
            retval.Monday = GetBoolVal((retval.Day >> 1) & 0x1);
            retval.Tuesday = GetBoolVal((retval.Day >> 2) & 0x1);
            retval.Wednesday = GetBoolVal((retval.Day >> 3) & 0x1);
            retval.Thursday = GetBoolVal((retval.Day >> 4) & 0x1);
            retval.Friday = GetBoolVal((retval.Day >> 5) & 0x1);
            retval.Saturday = GetBoolVal((retval.Day >> 6) & 0x1);
            if (retval.StartTimeHour >= 12)
                retval.StartTimePM = true;
            else if (retval.StartTimeHour == 0 && !retval.StartTimePM)
                retval.StartTimeHour = 12;
            if (retval.StartTimeHour > 12 && retval.StartTimePM)
                retval.StartTimeHour -= 12;
            if (retval.EndTimeHour >= 12)
                retval.EndTimePM = true;
            else if (retval.EndTimeHour == 0 && !retval.EndTimePM)
                retval.EndTimeHour = 12;
            if (retval.EndTimeHour > 12 && retval.EndTimePM)
                retval.EndTimeHour -= 12;

            return retval;
        }
        //----------------------------------------------
        //
        // GetGenre
        // Given a (Schedule)id, get the genre for this
        // program
        //
        // PARAMS: Integer (id == ScheduleID)
        // RETURNS: Integer (indicating the GenreID)
        //
        //-----------------------------------------------
        public int GetGenre(int id)
        {
            return conn.QuerySingle<int>("SELECT GENRE FROM SCHEDULE WHERE SCHEDULEID = @id",
                new { id = id });
        }
        //----------------------------------------------
        //
        // GetAllGenres
        // 
        // Return an IEnumerable-derived list of all genres
        //
        // PARAMS: None
        //
        // RETURNS: IEnumerable-derived list of integers of 
        // all genres
        //
        //---------------------------------------------------
        public IEnumerable<int> GetAllGenres()
        {
            return conn.QuerySingle<IEnumerable<int>>("SELECT GENREID FROM SCHEDULE");
        }
        //----------------------------------------------
        //
        // GetDayStr
        //
        // Given a program (Schedule object), 
        // get a string representing the day
        //
        // PARAMS: Schedule object representing a 
        // record
        // RETURNS: string representing the days of
        // the week when the program is scheduled to
        // record (e.g. "S", "TF","Sa" etc.)
        public string GetDayStr(Schedule program)
        {
            string retval = "";
            if (program.Sunday) retval += "S";
            if (program.Monday) retval += "M";
            if (program.Tuesday) retval += "T";
            if (program.Wednesday) retval += "W";
            if (program.Thursday) retval += "Th";
            if (program.Friday) retval += "F";
            if (program.Saturday) retval += "Sa";
            return retval;
        }
        public void UpdateTime(Schedule program)
        {
            if (program.StartTimePM && program.StartTimeHour == 0)
                program.starttime += "12:";
            else
                program.starttime += program.StartTimeHour.ToString() + ":";
            if (program.StartTimeMin < 10)
                program.starttime += "0";
            program.starttime += program.StartTimeMin.ToString() + " ";
            program.endtime += program.EndTimeHour.ToString() + ":";
            if (program.EndTimeMin < 10)
                program.endtime += "0";
            program.endtime += program.EndTimeMin.ToString() + " ";
            if (program.StartTimePM)
                program.starttime += "PM";
            else
                program.starttime += "AM";
            if (program.EndTimePM)
                program.endtime += "PM";
            else
                program.endtime += "AM";
            if (program.StartTimePM && program.StartTimeHour < 12)
                program.StartTimeHour += 12;
            else if (!program.StartTimePM && program.StartTimeHour == 12)
                program.StartTimeHour = 0;

            if (program.EndTimePM && program.EndTimeHour < 12)
                program.EndTimeHour += 12;
            else if (!program.EndTimePM && program.EndTimeHour == 12)
                program.EndTimeHour = 0;
        }
        //----------------------------------------------
        //
        // UpdateProgram
        // 
        // Given a program (Schedule object),
        // execute an SQL update reflecting the latest 
        // values
        // Make sure Day is calculated correctly, as 
        // well as StartTimeHour + EndTimeHour
        // PARAMS: program (Schedule object)
        // RETURNS: Nothing; the database is updated
        //
        //-----------------------------------------------
        public void UpdateProgram(Schedule program)
        {
            // Day is a binary #; Sunday = 2^0, Monday = 2^1, etc.
            // Calculate StartTimeHour + EndTimeHour using this algorithm:
            // If PM and hour < 12, add 12 to starthour/endhour
            // else if not PM and hour equals 12, then starthour/endhour 
            // equals 0
            program.Day = GetIntVal(program.Sunday) | (GetIntVal(program.Monday) * 2) | (GetIntVal(program.Tuesday) * 4) | (GetIntVal(program.Wednesday) * 8) | (GetIntVal(program.Thursday) * 16) | (GetIntVal(program.Friday) * 32) | (GetIntVal(program.Saturday) * 64);
            program.daystr = GetDayStr(program);
            UpdateTime(program);


            conn.Execute("UPDATE schedule SET ProgramName = @name, URL = @URL, Day = @Day, daystr = @daystr, StartTimeHour = @StartTimeHour, StartTimeMin = @StartTimeMin, EndTimeHour = @EndTimeHour, EndTimeMin=@EndTimeMin, Repeating = @Repeating, Shoutcast = @Shoutcast, Genre = @Genre, Password = @Password, Starttime = @Starttime, Endtime=@Endtime WHERE ScheduleID = @id",

                new { id = program.ScheduleID, name = program.ProgramName, URL = program.URL, Day = program.Day, daystr = program.daystr, StartTimeHour = program.StartTimeHour,
                    StartTimeMin = program.StartTimeMin, EndTimeHour = program.EndTimeHour, EndTimeMin = program.EndTimeMin,
                    Repeating = program.Repeating, Shoutcast = program.Shoutcast, Genre = program.GenreID, Password = program.Password,
                    Starttime = program.starttime, Endtime = program.endtime });
        }
        //----------------------------------------------------
        // InsertProgram
        // 
        // Given a program (Schedule object), execute an SQL
        // call to insert it into the database
        // Make sure Day is calculated correctly, as 
        // well as StartTimeHour + EndTimeHour
        // PARAMS: Schedule object (program)
        // RETURNS: Nothing; program is inserted as a record
        // in the database
        //
        //----------------------------------------------------
        public void InsertProgram(Schedule program)
        {
            program.Day = GetIntVal(program.Sunday) | (GetIntVal(program.Monday) * 2) | (GetIntVal(program.Tuesday) * 4) | (GetIntVal(program.Wednesday) * 8) | (GetIntVal(program.Thursday) * 16) | (GetIntVal(program.Friday) * 32) | (GetIntVal(program.Saturday) * 64);
            program.daystr = GetDayStr(program);
            program.starttime = "";
            program.endtime = "";

            UpdateTime(program);


            conn.Execute("INSERT INTO schedule (PROGRAMNAME, URL, DAY, DAYSTR, STARTTIMEHOUR,STARTTIMEMIN,ENDTIMEHOUR,ENDTIMEMIN,REPEATING,SHOUTCAST,GENRE,STATUS,PASSWORD,STARTTIME,ENDTIME,TIMEOUT,LASTMOD) " +
                "VALUES (@name, @URL,@day,@daystr,@starttimehour,@starttimemin,@endtimehour,@endtimemin,@repeating,@shoutcast,@genre,@status,@password,@starttime,@endtime,@timeout,@lastmod);",
                new
                {
                    name = program.ProgramName,
                    URL = program.URL,
                    day = program.Day,
                    daystr = program.daystr,
                    starttimehour = program.StartTimeHour,
                    starttimemin = program.StartTimeMin,
                    endtimehour = program.EndTimeHour,
                    endtimemin = program.EndTimeMin,
                    repeating = program.Repeating,
                    shoutcast = program.Shoutcast,
                    genre = program.GenreID,
                    status = "Queued",
                    password = program.Password,
                    starttime = program.starttime,
                    endtime = program.endtime,
                    timeout = 30,
                    lastmod = "1-1-1980 00:00:00"
                });
        }
        //----------------------------------------------------
        // GetGenres
        // Function invokes an SQL query that returns all genres
        // PARAMS: None
        // RETURNS: IEnumerable object
        //-----------------------------------------------------
        public IEnumerable<Genre> GetGenres()
        {
            return conn.Query<Genre>("SELECT * FROM GENRE;");
        }
        //----------------------------------------------------
        // GetRecstatuses
        // Function invokes an SQL query that returns all 
        // recording statuses
        // PARAMS: None
        // RETURNS: IEnumerable object
        //----------------------------------------------------
        public IEnumerable<Recstatus> GetRecstatuses()
        {
            return conn.Query<Recstatus>("SELECT * FROM RECSTATUS;");
        }
        //----------------------------------------------------
        // AssignGenre
        // Function assigns genres to program
        // PARAMS: None
        // RETURNS: Schedule object
        //----------------------------------------------------
        public Schedule AssignGenre()
        {
            var categoryList = GetGenres();
            var program = new Schedule();
           
            program.Genres = categoryList;

            return program;
        }
        //----------------------------------------------------
        // AssignStatus
        // Function assigns Recstatuses to program
        // PARAMS: None
        // RETURNS: Schedule object
        //----------------------------------------------------
        public Schedule AssignStatus()
        {
            var program = new Schedule();
            var statusList = GetRecstatuses();

            program.Recstatus = statusList;
            return program;
        }
        //-----------------------------------------------------
        // DeleteProgram
        // Function invokes SQL command to delete a program
        // whose schedule_id == id
        // PARAMS: id (int)
        // RETURNS: Nothing; record is deleted
        //------------------------------------------------------
        public void DeleteProgram(int id) //Schedule program)
        {
            conn.Execute("DELETE FROM SCHEDULE WHERE ScheduleID = @id; ", new { id = id }); // program.ScheduleID });
        }
        


    }
}