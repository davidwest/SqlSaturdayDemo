using System.Collections.Generic;
using System.Linq;
using FluidDbClient;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbKlugeInfoLookup : IKlugeInfoLookup
    {
        public IReadOnlyList<KlugeInfo> GetKluges(string search = null)
        {
            var query = GetQuery(search);

            return GetKluges(query);
        }

        public KlugeInfo GetKluge(int klugeId)
        {
            var query = GetQuery(klugeId);

            return GetKluges(query).Single();
        }

        private static IReadOnlyList<KlugeInfo> GetKluges(IManagedDbQuery query)
        {
            var partial1 = new List<KlugeInfo>();
            var partial2 = new List<KlugeInfo>();

            query.ProcessResultSets(recs => partial1 = recs.Buffer()
                                                           .MapNested(
                                                            rec => rec.Get<int>("KlugeId"),
                                                            rec => rec.Get<int>("RobotId"),
                                                            (rec, robots) =>
                                                            {
                                                                var kluge = rec.Map<KlugeInfo>();
                                                                kluge.Robots = robots.ToList();
                                                                return kluge;
                                                            },
                                                            rec => rec.Map<Robot>()).ToList(),
                                    recs => partial2 = recs.Buffer()
                                                           .MapNested(
                                                            rec => rec.Get<int>("KlugeId"),
                                                            rec => rec.Get<int>("ContraptionId"),
                                                            rec => rec.Get<int>("WidgetId"),
                                                            (rec, contraptions) =>
                                                            {
                                                                var kluge = rec.Map<KlugeInfo>();
                                                                kluge.Contraptions = contraptions.ToList();
                                                                return kluge;
                                                            },
                                                            (rec, widgets) =>
                                                            {
                                                                var contraption = rec.Map<ContraptionInfo>();
                                                                contraption.Widgets = widgets.ToList();
                                                                return contraption;
                                                            },
                                                            rec => rec.Map<Widget>()).ToList());

            var result = partial1.Zip(partial2, (p1, p2) =>
            {
                p1.Contraptions = p2.Contraptions;
                return p1;
            }).ToArray();

            return result;
        }
        
        private static ScriptDbQuery GetQuery(string search)
        {
            return !string.IsNullOrWhiteSpace(search) 
                ? new ScriptDbQuery(string.Format(ScriptTemplate, "WHERE k.[Name] LIKE '%' + @search + '%'"), new {search}) 
                : new ScriptDbQuery(string.Format(ScriptTemplate, ""));
        }

        private static ScriptDbQuery GetQuery(int klugeId)
        {
            return new ScriptDbQuery(string.Format(ScriptTemplate, "Where k.KlugeId = @klugeId"), new {klugeId});
        }

        private const string ScriptTemplate =
@"
SELECT k.KlugeId,
	   k.[Name],
	   r.RobotId,
	   r.[Name] AS Robot_Name,
	   r.[Description] AS Robot_Description,
	   r.DateBuilt AS Robot_DateBuilt,
	   r.DateDestroyed AS Robot_DateDestroyed,
	   r.IsEvil AS Robot_IsEvil
FROM Kluge AS k
LEFT JOIN KlugeRobot AS kr ON k.KlugeId = kr.KlugeId
LEFT JOIN Robot AS r ON kr.RobotId = r.RobotId
{0};

SELECT k.KlugeId, 
       c.ContraptionId, 
	   c.[Name],
	   w.WidgetId,
	   w.GlobalId AS Widget_GlobalId,
	   w.[Name] AS Widget_Name,
	   w.[Description] AS Widget_Description
FROM Kluge AS k
LEFT JOIN Contraption AS c ON k.KlugeId = c.KlugeId
LEFT JOIN ContraptionWidget AS cw ON c.ContraptionId = cw.ContraptionId
LEFT JOIN Widget AS w ON cw.WidgetId = w.WidgetId
{0};
";
    }
}
