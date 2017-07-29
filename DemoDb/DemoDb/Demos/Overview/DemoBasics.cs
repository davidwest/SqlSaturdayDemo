using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Demos.Overview
{
    public static class DemoBasics
    {
        private static readonly string[] Names = { "eve", "bender", "dalek", "wildcard" };
        private static readonly DateTime[] Dates = { new DateTime(1983, 6, 28), new DateTime(1992, 1, 25) };

        public static void Start()
        {
            Debug.WriteLine("\n--- Execute ---");
            DemoExecute();

            Debug.WriteLine("\n--- Scalar ---");
            DemoGetScalar();

            Debug.WriteLine("\n--- Single Result Set ---");
            DemoGetSingleResultSet();

            Debug.WriteLine("\n--- Multi Result Sets ---");
            DemoGetMultiResultSet();

            Debug.WriteLine("\n--- Enumerable Parameters ---");
            DemoEnumerableParameters();

            Debug.WriteLine("\n--- Enumerable Parameters Behind the Scenes ---");
            DemoEnumerableParameters_BehindTheScenes();
        }

        public static void DemoExecute()
        {
            Db.Execute("UPDATE Widget SET Name=@newName WHERE Name = @oldName;", new {newName="Jello Magic", oldName="Jello Wizard"});
        }

        public static void DemoGetScalar()
        {
            var result = 
                Db.GetScalar<string>("SELECT Name FROM Robot WHERE Name LIKE '%' + @name + '%';", new {name = "hal"});

            Debug.WriteLine(result);
        }

        public static void DemoGetSingleResultSet()
        {
            var results =
                Db.GetResultSet("SELECT * FROM Robot WHERE Name LIKE '%' + @name + '%';", new { name = "rob" });

            Display(results);
        }

        public static void DemoGetMultiResultSet()
        {
            const string script =
                "SELECT * FROM Robot WHERE DateBuilt < @latestDate; SELECT * FROM Widget WHERE Name LIKE '%' + @name + '%';";
            
            Db.ProcessResultSets(script, 
                                 new {latestDate = new DateTime(1981, 1, 1), name = "whirli"},
                                 recs => Display(recs),
                                 recs => Display(recs));
        }
        
        public static void DemoEnumerableParameters()
        {
            var results =
                Db.GetResultSet("SELECT * FROM Robot WHERE Name IN (@Names) OR DateBuilt IN (@Dates);", new { Names, Dates });

            Display(results);
        }

        public static void DemoEnumerableParameters_BehindTheScenes()
        {
            var query = new ScriptDbQuery("SELECT * FROM Robot WHERE Name IN (@Names) OR DateBuilt IN (@Dates);", new { Names, Dates });
            
            Debug.WriteLine(query.ToDiagnosticString());

            var results = query.GetResultSet();

            Display(results);
        }

        private static void Display(IEnumerable<IDataRecord> records)
        {
            records.ForEach(rec => Debug.WriteLine(rec.ToDiagnosticString()));
        }
    }
}
