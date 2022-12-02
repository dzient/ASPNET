//========================================
// David Zientara
// 11-25-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// Recstatus.cs
// A recstatus table that is constantly updated
// when SSR is running
// Now likely obsolete 



namespace SSR.Models
{
    public class Recstatus
    {
        // The primary key:
        public int StatusID;
        // This is the time/date:
        public string LastMod;
    }
}
