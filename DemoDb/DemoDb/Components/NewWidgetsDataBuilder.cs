using System.Data;
using Microsoft.SqlServer.Server;
using FluidDbClient.Sql;

namespace DemoDb
{
    public class NewWidgetsDataBuilder : StructuredDataBuilder
    {
        public NewWidgetsDataBuilder() :
            base("NewWidgets",
                 new SqlMetaData("GlobalId", SqlDbType.UniqueIdentifier), 
                 new SqlMetaData("Name", SqlDbType.NVarChar, 50),
                 new SqlMetaData("Description", SqlDbType.NVarChar, -1))
        { }
    }
}
