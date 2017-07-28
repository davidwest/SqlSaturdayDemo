using System;
using System.Diagnostics;
using DemoDb.Application;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace DemoDb.Demos.Overview
{
    public static class DemoToStructuredData
    {
        public static void Start()
        {
            var widgets = new[]
            {
                new Widget(Guid.NewGuid(), "Everything Juicer", "It juices everything"),
                new Widget(Guid.NewGuid(), "80-Tooth Gear", "Keep it grinding!")
            };

            // Note that this is only possible because we're explicitly naming the fields (leaving WidgetId out)

            Db.Execute("INSERT INTO Widget (GlobalId, Name, Description) SELECT GlobalId, Name, Description FROM @data;",
                        new
                        {
                            data = widgets.ToStructuredData()
                        });
            
            Debug.WriteLine("\n!!! Inserted New Widgets !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Widget;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
