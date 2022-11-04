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

        public RecstatusRepository(IDbConnection _conn)
        {
            conn = _conn;
        }

        public Recstatus GetLastmod(int id)
        {
            return conn.QuerySingle<Recstatus>("SELECT LASTMOD FROM RECSTATUS WHERE STATUSID = @id",
                new { id = id });
        }

        public IEnumerable<Recstatus> GetAllRecstatuses()
        {
            return conn.Query<Recstatus>("SELECT * FROM RECSTATUS;");

        }
    }
}
