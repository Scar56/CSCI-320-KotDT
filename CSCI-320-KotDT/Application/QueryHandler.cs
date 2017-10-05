using System.Web.Http.Dispatcher;
using System;
using System.Runtime.InteropServices;
using System.Web;
using ControllerDI.Interfaces;
using Npgsql;

namespace ControllerDI.Services {
    public class QueryHandler : IQueryHandler {
        private NpgsqlConnection conn;
        public QueryHandler () {
            conn = new NpgsqlConnection("Host=reddwarf.cs.rit.edu; username=p32003f; password=kiexeiH7veiqu9Uta6Go");
            
            conn.Open();
        }

        public NpgsqlCommand query(string queryString) {
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = queryString;
            return cmd;
        }

    }
}