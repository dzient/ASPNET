//========================================
// David Zientara
// 10-5-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// ScheduleContoller.cs
// This is the request handler class
// Enables us to manipulate the database

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SSR.Models;

namespace SSR.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleRepository repo;
        //-------------------------------------------
        // ScheduleController
        // Constructor for this class
        // PARAMS: IScheduleRepository object
        // RETURNS: Nothing; class is initialized
        //
        //--------------------------------------------
        public ScheduleController(IScheduleRepository repo)
        {
            this.repo = repo;
        }
        //---------------------------------------------
        // Index
        // Program returns the contents of the Schedule
        // table
        // PARAMS: Nothing
        // RETURNS: IActionResult object containing 
        // the table
        //---------------------------------------------
        public IActionResult Index()
        {
            var programs = repo.GetAllPrograms();
        
            return View(programs);
        }
        //---------------------------------------------
        // ViewProgram
        // Program takes schedule ID as a parameter
        // and returns the program
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the program
        public IActionResult ViewProgram(int id)
        {
            var program = repo.GetProgram(id);

            return View(program);
        }

        public IActionResult UpdateProgram(int id)
        {
            Schedule program = repo.GetProgram(id);
            program.GenreID = repo.GetGenre(id);

            program.Genres = repo.GetGenres();

            if (program == null)
            {
                return View("ProgramNotFound");
            }
            return View(program);
        }
        public IActionResult UpdateProgramToDatabase(Schedule program)
        {
            repo.UpdateProgram(program);

            return RedirectToAction("ViewProgram", new { id = program.ScheduleID });

        }

        public IActionResult InsertProgram()
        {
            var program = repo.AssignGenre();

            return View(program);
        }

        public IActionResult InsertProgramToDatabase(Schedule programToInsert)
        {
            repo.InsertProgram(programToInsert);

            return RedirectToAction("Index");
        }
        public IActionResult DeleteProgram(int id) //Schedule program)
        {
            repo.DeleteProgram(id); // program);

            return RedirectToAction("Index");
        }

    }
}
