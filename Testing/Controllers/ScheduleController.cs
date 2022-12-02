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
            repo.GetRecstatuses();
        
            return View(programs);
        }
        //---------------------------------------------
        // ViewProgram
        // Program takes schedule ID as a parameter
        // and returns the program
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the program
        //----------------------------------------------
        public IActionResult ViewProgram(int id)
        {
            var program = repo.GetProgram(id);

            return View(program);
        }
        //---------------------------------------------
        // UpdateProgram
        // Program takes schedule ID as a parameter
        // and returns a view to the program
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the program
        //----------------------------------------------
        public IActionResult UpdateProgram(int id)
        {
            // 
            Schedule program = repo.GetProgram(id);
            program.GenreID = repo.GetGenre(id);

            program.Genres = repo.GetGenres();

            if (program == null)
            {
                return View("ProgramNotFound");
            }
            return View(program);
        }
        //---------------------------------------------
        // UpdateProgramToDatabase
        // Program takes schedule ID as a parameter
        // and returns the program
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the program
        //----------------------------------------------
        public IActionResult UpdateProgramToDatabase(Schedule program)
        {
            repo.UpdateProgram(program);

            return RedirectToAction("ViewProgram", new { id = program.ScheduleID });

        }
        //---------------------------------------------
        // InsertProgram
        // Program takes schedule ID as a parameter
        // and returns the program
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the program
        //----------------------------------------------
        public IActionResult InsertProgram()
        {
            var program = repo.AssignGenre();

            return View(program);
        }
        //---------------------------------------------
        // InsertProgramToDatabase
        // Program takes schedule as a parameter
        // and returns index
        // PARAMS: id (integer)
        // RETURNS: IActionResult object containing
        // the index
        //----------------------------------------------
        public IActionResult InsertProgramToDatabase(Schedule programToInsert)
        {
            repo.InsertProgram(programToInsert);

            return RedirectToAction("Index");
        }
        //------------------------------------------------
        // DeleteProgram
        // Program takes an id representing the ScheduleID
        // and deletes the record
        // PARMS: id (integer)
        // RETURNS: IActionResult object containing the
        // index
        //------------------------------------------------
        public IActionResult DeleteProgram(int id) //Schedule program)
        {
            repo.DeleteProgram(id); // program);

            return RedirectToAction("Index");
        }

    }
}
