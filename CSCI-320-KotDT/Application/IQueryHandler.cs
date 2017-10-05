using System.Linq;
using Npgsql;

namespace ControllerDI.Interfaces {
    public interface IQueryHandler {
        NpgsqlCommand query(string queryString);
    }
}