using System.Diagnostics;
using System.Transactions;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbSaveNewKlugeCommandHandler2 : ISaveNewKlugeCommandHandler
    {
        private readonly TransactionScope _txScope;

        public DbSaveNewKlugeCommandHandler2(TransactionScope txScope)
        {
            _txScope = txScope;
        }

        public int Execute(SaveNewKlugeCommand cmd)
        {
            var kluge = cmd.NewKluge;

            SaveNew(kluge);

            _txScope.Complete();

            return kluge.KlugeId;
        }

        private static void SaveNew(Kluge kluge)
        {
            var klugeId =
                Db.GetScalar<int>("INSERT INTO Kluge (Name) OUTPUT Inserted.KlugeId VALUES (@Name);", 
                                  new { kluge.Name });

            kluge.KlugeId = klugeId;

            foreach (var contraption in kluge.Contraptions)
            {
                var contraptionId = 
                    Db.GetScalar<int>("INSERT INTO Contraption (KlugeId, Name) OUTPUT Inserted.ContraptionId VALUES (@KlugeId, @Name);",
                                      new { kluge.KlugeId, contraption.Name });

                contraption.ContraptionId = contraptionId;
            }

            var compiler = new DbScriptCompiler();

            foreach (var robotId in kluge.RobotIds)
            {
                compiler.Append("INSERT INTO KlugeRobot (KlugeId,RobotId) VALUES ({0}, {1});", kluge.KlugeId, robotId);
            }

            foreach (var contraption in kluge.Contraptions)
            {
                foreach (var widgetId in contraption.WidgetIds)
                {
                    compiler.Append("INSERT INTO ContraptionWidget (ContraptionId,WidgetId) VALUES ({0}, {1});", 
                                    contraption.ContraptionId,
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
