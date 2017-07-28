using System;
using System.Diagnostics;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace DemoDb.Demos.Overview
{
    public class NewRobotDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateBuilt { get; set; }
        public bool IsEvil { get; set; }
    }
    
    public static class DemoToStructuredDataWithTypeName
    {
        public static void Start()
        {
            var newRobots = new[]
            {
                new NewRobotDto {Name = "Smokey", Description = "The robot version of the bear?", DateBuilt = DateTime.Now},
                new NewRobotDto {Name = "Mr. Roboto", Description = "Domo aryigato, mr. roboto!", DateBuilt = new DateTime(1981,3,21)}
            };
            
            Db.Execute("INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT * FROM @data;", 
                       new
                       {
                           data = newRobots.ToStructuredData("NewRobots")
                       });
            
            Debug.WriteLine("\n!!! Inserted New Robots !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Robot;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
