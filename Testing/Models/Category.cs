//========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// Category.cs
// A class for categories
// Contains CategoryID (int, primary key) and Name

using System;
namespace SSR.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}

