using System;
using System.Diagnostics;
using DemoDb.Application;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace DemoDb.Demos.Overview
{
    public class DemoCombineStructuredDataAndMultiParam
    {
        public static void Start()
        {
            var widgetsToAdd = new[]
            {
                new Widget(Guid.NewGuid(), "Everything Juicer", "It juices everything"),
                new Widget(Guid.NewGuid(), "80-Tooth Gear", "Keep it grinding!")
            };

            var widgetsToDelete = new [] {"Spiralizer", "Logic Monkey"};
            
            Db.Execute(Script, new
            {
                widgetsToAdd = widgetsToAdd.ToStructuredData(),
                widgetsToDelete
            });

            Debug.WriteLine("\n!!! Inserted And Deleted Widgets !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Widget;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }

        private const string Script =
@"
INSERT INTO Widget (GlobalId, Name, Description) SELECT GlobalId, Name, Description FROM @widgetsToAdd;
DELETE FROM Widget WHERE Name IN (@widgetsToDelete);
";
    }
}
