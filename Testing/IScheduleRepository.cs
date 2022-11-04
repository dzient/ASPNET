//========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// Interface defining methods that inherited
// classes must implement
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SSR.Models;

namespace SSR.Models
{

    public interface IScheduleRepository
    {
        // Get an IEnumerable-derived object listing all programs:
        public IEnumerable<Schedule> GetAllPrograms();

        //Get a single object representing a program:
        public Schedule GetProgram(int id);
        // Update the program:
        public void UpdateProgram(Schedule schedule);
        
        //public void AddProgram(Schedule programToInsert);
        // Get an IEnumerable-derived object listing all genres:
        public IEnumerable<Genre> GetGenres();
        //Assign a genre (this is a kludge):
        public Schedule AssignGenre();  
        // Insert a program into the database:
        public void InsertProgram(Schedule programToInsert);
        //Delete a program from the database:
        public void DeleteProgram(int id); // Schedule program);
        // Get a single genre:
        public int GetGenre(int id);
        //Get all genres from the database:
        public IEnumerable<int> GetAllGenres();

        public IEnumerable<Recstatus> GetRecstatuses();
    }

}