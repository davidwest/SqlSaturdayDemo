using System;
using System.Collections.Generic;
using DemoDb.Application;

namespace DemoDb.Demos.App
{
    public static class Extensions
    {
        public static List<int> PickThreeUniqueIds(this IReadOnlyList<Widget> widgets, Random rnd)
        {
            var result = new List<int>();

            while (result.Count < 3)
            {
                var next = rnd.Next(0, widgets.Count - 1);

                var widgetId = widgets[next].WidgetId;

                if (!result.Contains(widgetId))
                {
                    result.Add(widgetId);
                }
            }

            return result;
        }
        
        public static int PickOneIdNotIn(this IReadOnlyList<Robot> robots, List<int> existingIds, Random rnd)
        {
            var widgetId = 0;

            while (widgetId == 0)
            {
                var next = rnd.Next(0, robots.Count - 1);

                widgetId = robots[next].RobotId;

                if (existingIds.Contains(widgetId))
                {
                    widgetId = 0;
                }
            }

            return widgetId;
        }
    }
}
