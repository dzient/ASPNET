﻿//============================================
// David Zientara
// 11-25-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// RecstatusRepository.cs
// Retrieves data from Recstatus
//--------------------------------------------

using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using SSR.Models;

namespace SSR.Models
{
    public class RecstatusRepository : Controller
    {
        private readonly IDbConnection conn;
        //-----------------------------------------------
        // RecstatusRepository
        // Constructor for the class
        // PARAMS: _conn (IDbConnection_
        // RETURNS: Nothing
        //--------------------------------------------
        public RecstatusRepository(IDbConnection _conn)
        {
            conn = _conn;
        }
        //-----------------------------------------------
        // GetLastMod
        // Function invokes SQL query for a single record
        // PARAMS: id (int)
        // RETURNS: Recstatus object 
        //
        //--------------------------------------------
        public Recstatus GetLastmod(int id)
        {
            return conn.QuerySingle<Recstatus>("SELECT LASTMOD FROM RECSTATUS WHERE STATUSID = @id",
                new { id = id });
        }
        //-----------------------------------------------
        // GetAllRecstatuses
        // Function invokes SQL query for all records
        // PARAMS: id 
        // RETURNS: IEnumerable object  
        //--------------------------------------------
        public IEnumerable<Recstatus> GetAllRecstatuses()
        {
            return conn.Query<Recstatus>("SELECT * FROM RECSTATUS;");

        }
    }
}
