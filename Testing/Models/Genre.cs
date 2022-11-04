//========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// Genre.cs
// Class has variables for genres for programs
// Genre ID = auto-incrementing ID
// Category = the actual genre category
// (1 = podcast; 2 = talk, etc.)

namespace SSR.Models
{
    public class Genre
    {
        public int GenreID;
        public string Category;
    }
}
