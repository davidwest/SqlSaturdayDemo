using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbRobotLookup : IRobotLookup
    {
        public IReadOnlyList<Robot> GetRobots(string search = null)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return MapRobots(Db.GetResultSet("SELECT * FROM Robot;"));
            }

            const string script = 
                "SELECT * FROM Robot WHERE Name LIKE '%' + @search + '%' OR Description LIKE '%' + @search + '%';";

            return MapRobots(Db.GetResultSet(script, new {search}));
        }

        private static IReadOnlyList<Robot> MapRobots(IEnumerable<IDataRecord> records)
        {
            return records.Map<Robot>().ToArray();
        }
    }
}
