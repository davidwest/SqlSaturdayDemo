using System.Linq;
using System.Text;
using DemoDb.Application;

namespace DemoDb.Demos.Overview
{
    public static class DisplayExtensions
    {
        public static string ToDisplayString(this Robot robot)
        {
            return $"{robot.RobotId, -6}  Is evil: {robot.IsEvil, -8} {robot.Name, -30} {robot.DateBuilt.ToString("d"), -15}    {robot.Description}";
        }

        public static string ToDisplayString(this ImmutableRobot robot)
        {
            return $"{robot.RobotId,-6}   Is good: {robot.IsGood, -8} {robot.Name,-30} {robot.DateBuilt.ToString("d"),-15}    {robot.Description}";
        }

        public static string ToDisplayString(this Widget widget)
        {
            return $"{widget.WidgetId,-6} {widget.GlobalId}  {widget.Name,-30} {widget.Description}";
        }

        public static string ToDisplayString(this Kluge kluge)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"\n{kluge.KlugeId,-6} {kluge.Name}");
            builder.AppendLine("Robot Ids:");

            foreach (var robotId in kluge.RobotIds.OrderBy(id => id))
            {
                builder.AppendLine($"     {robotId}");
            }

            builder.AppendLine();

            foreach (var c in kluge.Contraptions.OrderBy(c => c.Name))
            {
                builder.AppendLine($"     {c.ContraptionId,-6} {c.Name}");
                builder.AppendLine("     Widget Ids:");

                foreach (var widgetId in c.WidgetIds.OrderBy(id => id))
                {
                    builder.AppendLine($"          {widgetId}");
                }
            }

            return builder.ToString();
        }

        public static string ToDisplayString(this KlugeInfo kluge)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"\n{kluge.KlugeId, -6} {kluge.Name}");

            foreach (var robot in kluge.Robots.OrderBy(r => r.RobotId))
            {
                builder.AppendLine($"     {robot.ToDisplayString()}");
            }

            builder.AppendLine();

            foreach (var c in kluge.Contraptions.OrderBy(c => c.ContraptionId))
            {
                builder.AppendLine($"     {c.ContraptionId, -6} {c.Name}");

                foreach (var w in c.Widgets.OrderBy(w => w.WidgetId))
                {
                    builder.AppendLine($"          {w.ToDisplayString()}");
                }
            }

            return builder.ToString();
        }
        
        public static string ToDisplayString(this ContactInfo item)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"{item.Address.StreetNumber} {item.Address.StreetName}");
            builder.AppendLine($"{item.Address.Locale.City}, {item.Address.Locale.State} {item.Address.Locale.Zip}");
            builder.AppendLine($"({item.Phone.AreaCode}) {item.Phone.Prefix}-{item.Phone.Postfix}");
            return builder.ToString();
        }
    }
}
