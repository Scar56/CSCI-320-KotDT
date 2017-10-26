using System.Web.Http.Dispatcher;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Web;
using ControllerDI.Interfaces;
using Npgsql;

namespace ControllerDI.Services {
    public class QueryHandler : IQueryHandler {
        private readonly NpgsqlConnection conn;
        
        // Constructor, opens the connection to our database 
        public QueryHandler () {
            conn = new NpgsqlConnection("Host=reddwarf.cs.rit.edu; username=p32003f; password=kiexeiH7veiqu9Uta6Go");
            conn.Open();
        }

        /// <summary>creates command object for specified query</summary>
        /// <returns> NpgsqlCommand for our connection string and the desired query</returns>
        /// <param name="queryString">The desired query string</param>
        
        public NpgsqlCommand query(string queryString) {
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = queryString;
            return cmd;
        }

        public ArrayList read(string queryString, int columns) {
            var cmd = query(queryString);
            var dr = cmd.ExecuteReader();
            ArrayList res = new ArrayList();
            while (dr.Read()) {
                if (columns == 1)
                    res.Add(dr[0]);
                else {
                    ArrayList row = new ArrayList();
                    for (int i = 0; i < columns; i++) {
                        row.Add(dr[i]);
                    }
                    res.Add(row);
                }
            }
            dr.Close();
            return res;
        }

    }
}