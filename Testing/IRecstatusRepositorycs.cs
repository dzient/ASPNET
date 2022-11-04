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
