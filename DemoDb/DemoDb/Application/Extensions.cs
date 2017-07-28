using System.Linq;

namespace DemoDb.Application
{
    public static class Extensions
    {
        // think of this as a query to command model conversion

        // ... which is not a realistic thing in most cases, buts serves our purposes ...

        public static Kluge ToKluge(this KlugeInfo src)
        {
            return new Kluge
            {
                KlugeId = src.KlugeId,
                Name = src.Name,
                RobotIds = src.Robots
                           .Select(r => r.RobotId)
                           .ToList(),
                Contraptions = src.Contraptions
                               .Select(c => new Contraption
                               {
                                    ContraptionId = c.ContraptionId,
                                    Name = c.Name,
                                    WidgetIds = c.Widgets
                                                .Select(w => w.WidgetId)
                                                .ToList()
                               }).ToList()
            };
        }
    }
}
