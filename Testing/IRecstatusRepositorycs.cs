//============================================
// David Zientara
// 11-25-2022
// ASP.NET application to add a web interface 
// to my stream recorder 
//
// IRecstatusRepository.cs
// Interface for RecstatusRepository.cs
// Retrieves data from Recstatus
//--------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SSR.Models;


namespace SSR.Models
{
    public interface IRecstatusRepository
    {
        public Recstatus GetLastmod(int id);
        public IEnumerable<Recstatus> GetAllRecstatuses();

    }
}
