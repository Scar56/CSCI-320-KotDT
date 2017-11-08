using System.Collections;
using Npgsql;

namespace ControllerDI.Interfaces {
    public interface IQueryHandler {
        NpgsqlCommand query(string queryString);
        NpgsqlCommand query(string queryString, params object[] parameters);
        ArrayList read(string queryString, int columns);
        void nonQuery(string queryString);
    }
}