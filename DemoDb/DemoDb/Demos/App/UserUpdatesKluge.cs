using System;
using System.Diagnostics;
using System.Linq;
using DemoDb.Application;
using DemoDb.Demos.Overview;

namespace DemoDb.Demos.App
{
    public static class UserUpdatesKluge
    {
        private static readonly IRobotLookup RobotLookup = AppServices.Get<IRobotLookup>();
        private static readonly IWidgetLookup WidgetLookup = AppServices.Get<IWidgetLookup>();
        private static readonly IKlugeInfoLookup KlugeInfoLookup = AppServices.Get<IKlugeInfoLookup>();

        public static void Start()
        {
            var kluge = KlugeInfoLookup.GetKluges("Rube").Single().ToKluge();

            Debug.WriteLine("*** BEFORE ***");
            Debug.WriteLine(kluge.ToDisplayString());

            UserMakesChanges(kluge);

            Debug.WriteLine("*** AFTER ***");
            Debug.WriteLine(kluge.ToDisplayString());

            UserSaves(kluge);
        }

        private static void UserSaves(Kluge kluge)
        {
            var cmd = new SaveUpdatedKlugeCommand { UpdatedKluge = kluge };

            AppServices.Use<ISaveUpdatedKlugeCommandHandler>(handler =>
            {
                handler.Execute(cmd);
            });
        }

        public static void UserMakesChanges(Kluge kluge)
        {
            var rnd = new Random();

            UserRemovesOneRobot(kluge);

            UserAddsOneRobot(kluge, rnd);

            UserRemovesOneContraption(kluge);

            UserAddsOneWidgetToFirstContraption(kluge);

            UserRemovesOneWidgetFromLastContraption(kluge);

            UserAddsOneNewContraption(kluge, rnd);
        }

        private static void UserRemovesOneRobot(Kluge kluge)
        {
            if (kluge.RobotIds.Count == 0) return;
            
            kluge.RobotIds.RemoveAt(0);
            
        }
        
        private static void UserAddsOneRobot(Kluge kluge, Random rnd)
        {
            var robots = RobotLookup.GetRobots();

            if (kluge.RobotIds.Count == 0)
            {
                kluge.RobotIds.Add(robots.First().RobotId);
                return;
            }

            var uniqueRobotId = robots.PickOneIdNotIn(kluge.RobotIds, rnd);

            kluge.RobotIds.Add(uniqueRobotId);
        }

        private static void UserRemovesOneContraption(Kluge kluge)
        {
            if (kluge.Contraptions.Count == 0) return;
            
            kluge.Contraptions.RemoveAt(0);
            
        }

        private static void UserAddsOneWidgetToFirstContraption(Kluge kluge)
        {
            if (kluge.Contraptions.Count == 0) return;

            var contraption = kluge.Contraptions[0];

            var uniqueWidgetId = 
                WidgetLookup.GetWidgets()
                .Select(w => w.WidgetId)
                .Except(contraption.WidgetIds)
                .First();

            contraption.WidgetIds.Add(uniqueWidgetId);
        }
        
        private static void UserRemovesOneWidgetFromLastContraption(Kluge kluge)
        {
            if (kluge.Contraptions.Count == 0) return;

            var contraption = kluge.Contraptions.Last();

            if (contraption.WidgetIds.Count == 0) return;
            
            contraption.WidgetIds.RemoveAt(0);
        }

        private static void UserAddsOneNewContraption(Kluge kluge, Random rnd)
        {
            var contraption = new Contraption
            {
                Name = "KaPow",
                WidgetIds = WidgetLookup.GetWidgets().PickThreeUniqueIds(rnd)
            };

            kluge.Contraptions.Add(contraption);
        }
    }
}
