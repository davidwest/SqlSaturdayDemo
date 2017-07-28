using System.Diagnostics;
using System.Linq;
using System.Transactions;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbSaveUpdatedKlugeCommandHandler : ISaveUpdatedKlugeCommandHandler
    {
        private readonly IKlugeInfoLookup _lookup;
        private readonly TransactionScope _txScope;
        private readonly KlugeChangeDetector _changeDetector;

        public DbSaveUpdatedKlugeCommandHandler(IKlugeInfoLookup lookup, TransactionScope txScope)
        {
            _lookup = lookup;
            _txScope = txScope;
            _changeDetector = new KlugeChangeDetector();
        }

        public void Execute(SaveUpdatedKlugeCommand cmd)
        {
            var updated = cmd.UpdatedKluge;
            var orig = _lookup.GetKluge(cmd.UpdatedKluge.KlugeId).ToKluge();

            Update(orig, updated);

            _txScope.Complete();
        }

        private void Update(Kluge orig, Kluge updated)
        {
            var changes = _changeDetector.DetectChanges(orig, updated);

            foreach (var contraption in changes.AddedContraptions)
            {
                var contraptionId =
                    Db.GetScalar<int>("INSERT INTO Contraption (KlugeId, Name) OUTPUT Inserted.ContraptionId VALUES (@KlugeId, @Name);",
                                      new { updated.KlugeId, contraption.Name });

                contraption.ContraptionId = contraptionId;
            }

            var compiler = new DbScriptCompiler();

            foreach (var contraption in changes.AddedContraptions)
            {
                foreach (var widgetId in contraption.WidgetIds)
                {
                    compiler.Append("INSERT INTO ContraptionWidget (ContraptionId,WidgetId) VALUES ({0}, {1});",
                                    contraption.ContraptionId,
                                    widgetId);
                }
            }

            foreach (var robotId in changes.AddedRobotIds)
            {
                compiler.Append("INSERT INTO KlugeRobot (KlugeId,RobotId) VALUES ({0}, {1});", updated.KlugeId, robotId);
            }

            foreach (var robotId in changes.RemovedRobotIds)
            {
                compiler.Append("DELETE FROM KlugeRobot WHERE KlugeId={0} AND RobotId={1};", updated.KlugeId, robotId);
            }

            foreach (var contraptionId in changes.RemovedContraptionIds)
            {
                var origContraption = orig.Contraptions.Single(c => c.ContraptionId == contraptionId);

                foreach (var widgetId in origContraption.WidgetIds)
                {
                    compiler.Append("DELETE FROM ContraptionWidget WHERE ContraptionId={0} AND WidgetId={1};",
                                    contraptionId,
                                    widgetId);
                }

                compiler.Append("DELETE FROM Contraption WHERE ContraptionId={0};", contraptionId);
            }

            foreach (var widgetsAddedKvp in changes.WidgetsAddedToExistingContraptions)
            {
                var contraptionId = widgetsAddedKvp.Key;

                foreach (var widgetId in widgetsAddedKvp.Value)
                {
                    compiler.Append("INSERT INTO ContraptionWidget (ContraptionId,WidgetId) VALUES ({0}, {1});",
                                    contraptionId,
                                    widgetId);
                }
            }

            foreach (var widgetsRemovedKvp in changes.WidgetsRemovedFromExistingContraptions)
            {
                var contraptionId = widgetsRemovedKvp.Key;

                foreach (var widgetId in widgetsRemovedKvp.Value)
                {
                    compiler.Append("DELETE FROM ContraptionWidget WHERE ContraptionId={0} AND WidgetId={1};",
                                    contraptionId,
                                    widgetId);
                }
            }

            var cmd = new ScriptDbCommand();
            cmd.IncludeScriptDoc(compiler.Compile());

            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();
        }
    }
}
