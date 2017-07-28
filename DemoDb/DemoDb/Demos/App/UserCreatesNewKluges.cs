using System;
using System.Linq;
using DemoDb.Application;
using FluidDbClient.Shell;

namespace DemoDb.Demos.App
{
    public static class UserCreatesNewKluges
    {
        private static readonly IRobotLookup RobotLookup = AppServices.Get<IRobotLookup>();
        private static readonly IWidgetLookup WidgetLookup = AppServices.Get<IWidgetLookup>();

        public static void Start()
        {
            ClearExistingKluges();
            
            var rnd = new Random();
            
            var kluge1 = UserCreatesKluge("Rube Goldberg Device", "x", rnd);
            UserSaves(kluge1);

            var kluge2 = UserCreatesKluge("Power Grid", "y", rnd);
            UserSaves(kluge2);

            var kluge3 = UserCreatesKluge("Bug Repairs", "z", rnd);
            UserSaves(kluge3);
        }
        
        private static void UserSaves(Kluge kluge)
        {
            var cmd = new SaveNewKlugeCommand { NewKluge = kluge };

            AppServices.Use<ISaveNewKlugeCommandHandler>(handler =>
            {
                handler.Execute(cmd);
            });
        }

        private static Kluge UserCreatesKluge(string name, string robotSearch, Random rnd)
        {
            var widgets = WidgetLookup.GetWidgets();
            var robots = RobotLookup.GetRobots(robotSearch).ToArray();

            var robotIds = robots.Select(r => r.RobotId).ToList();

            // lets get crazy:

            var contraptions =
                from r in robots.Take(3)
                let contraptionName = new string(r.Name.ToUpper().Reverse().ToArray())
                let widgetIds = widgets.PickThreeUniqueIds(rnd)
                select new Contraption
                {
                    Name = contraptionName,
                    WidgetIds = widgetIds
                };
            
            return new Kluge
            {
                Name = name,
                RobotIds = robotIds,
                Contraptions = contraptions.ToList()
            };
        }
        
        private static void ClearExistingKluges()
        {
            Db.Execute("DELETE FROM KlugeRobot; DELETE FROM ContraptionWidget; DELETE FROM Contraption; DELETE FROM Kluge;");
        }
    }
}
