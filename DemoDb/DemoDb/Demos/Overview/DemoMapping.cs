using System;
using System.Diagnostics;
using System.Linq;
using DemoDb.Application;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Demos.Overview
{
    public static class DemoMapping
    {
        public static void Start()
        {
            Debug.WriteLine("\n--- Simple Mapping ---");
            DemoSimpleMapping();

            Debug.WriteLine("\n--- Multi Mapping ---");
            DemoMultiMapping();

            Debug.WriteLine("\n--- Explicit Mapping ---");
            DemoExplitMapping();
        }

        private static void DemoSimpleMapping()
        {
            var robot = Db.GetRecord("SELECT RobotId = 1, Name = 'Roboto'").Map<Robot>();

            Debug.WriteLine(robot.ToDisplayString());
        }

        private static void DemoMultiMapping()
        {
            var items =
                Db.GetResultSet("SELECT * FROM ContactInfo")
                .Map<ContactInfo, Locale, MailingAddress, PhoneNumber>((ci, l, ma, ph) =>
                {
                    ma.Locale = l;
                    ci.Address = ma;
                    ci.Phone = ph;
                });

            items.ForEach(item => Debug.WriteLine(item.ToDisplayString()));
        }

        private static void DemoExplitMapping()
        {
            var robots = 
                Db.GetResultSet("SELECT * FROM Robot;")
                .Select(rec => new ImmutableRobot(rec.Get<int>("robotId"), 
                                                  rec.Get<string>("name"), 
                                                  rec.Get<string>("description"), 
                                                  rec.Get<DateTime>("dateBuilt"),
                                                  rec.Get<DateTime?>("dateDestroyed"), 
                                                  rec.Get<bool>("isEvil")));

            robots.ForEach(r => Debug.WriteLine(r.ToDisplayString()));
        }
    }
}
