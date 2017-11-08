using System.Collections;
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

        public NpgsqlCommand query(string queryString, params object[] parameters)
        {
            var cmd = new NpgsqlCommand(queryString, conn);
            for(int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.Add(new NpgsqlParameter("@" + i.ToString(), parameters[i]));
            }
            
            return cmd;
        }

        /// <summary>creates command object for specified query</summary>
        /// <returns>2D arraylist representation of query, or 1d arraylist if only one column</returns>
        /// <param name="queryString">The desired query string</param>
        /// <param name="columns">The number of columns in the result table</param>
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
        
        /// <summary>creates command object for specified query</summary>
        /// <param name="queryString">The desired query string</param>
        public void nonQuery(string queryString){
            var cmd = query(queryString);
            cmd.ExecuteNonQuery();
        }
        
    }
}