using System.Collections.Generic;
using System.Linq;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbWidgetLookup : IWidgetLookup
    {
        public IReadOnlyList<Widget> GetWidgets()
        {
            return Db.GetResultSet("SELECT * FROM Widget;")
                   .Select(rec => rec.Map<Widget>()).ToArray();
        }
    }
}
