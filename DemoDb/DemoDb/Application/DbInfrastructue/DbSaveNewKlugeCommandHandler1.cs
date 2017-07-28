using System.Diagnostics;
using FluidDbClient;
using FluidDbClient.Shell;

namespace DemoDb.Application.DbInfrastructue
{
    public class DbSaveNewKlugeCommandHandler1 : ISaveNewKlugeCommandHandler
    {
        private readonly DbSession _session;

        public DbSaveNewKlugeCommandHandler1(DbSession session)
        {
            _session = session;
        }
        
        public int Execute(SaveNewKlugeCommand cmd)
        {
            var kluge = cmd.NewKluge;

            SaveNew(kluge);

            _session.Commit();

            return kluge.KlugeId;
        }

        private void SaveNew(Kluge kluge)
        {
            var klugeId =
                Db.GetScalar<int>(_session,
                                  "INSERT INTO Kluge (Name) OUTPUT Inserted.KlugeId VALUES (@Name);", 
                                  new { kluge.Name });

            kluge.KlugeId = klugeId;

            foreach (var contraption in kluge.Contraptions)
            {
                var contraptionId = 
                    Db.GetScalar<int>(_session,
                                      "INSERT INTO Contraption (KlugeId, Name) OUTPUT Inserted.ContraptionId VALUES (@KlugeId, @Name);",
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

            var cmd = new ScriptDbCommand(_session);
            cmd.IncludeScriptDoc(compiler.Compile());

            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();
        }
    }
}
