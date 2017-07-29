using System;
using System.Linq;
using System.Transactions;
using DemoDb.Application;
using FluidDbClient;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace DemoDb.Demos.Overview
{
    public static class DemoMultiDbOperation
    {
        public static void Start()
        {
            CreateSleevesForWidgets();

            //UsingTransactionScope(CreateSleevesForWidgets);
        }

        private static WidgetSleeve CreateWidgetSleeve(Widget w)
        {
            return new WidgetSleeve
            {
                AcmeWidgetId = w.GlobalId,
                AcmeWidgetName = w.Name,
                Description = $"Handily encloses your Acme {w.Name} for shipping",
                AcmeWidgetDescription = w.Description.Contains(" and ") ? null : w.Description
            };
        }

        public static void UsingTransactionScope(Action doit)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var tscope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                doit();

                tscope.Complete();
            }
        }
        
        private static void CreateSleevesForWidgets()
        {
            var widgets =
                Db<AcmeDb>
                .GetResultSet("SELECT * FROM Widget;")
                .Map<Widget>();

            var widgetSleeves =
                widgets
                .Select(CreateWidgetSleeve);

            Db<BubbleWrapDb>.Execute(WriteSleevesScript, new
            {
                data = widgetSleeves.ToStructuredData()
            });
        }

        private const string WriteSleevesScript = 
@"
DELETE FROM WidgetSleeve; 
INSERT INTO WidgetSleeve (AcmeWidgetId, AcmeWidgetName, AcmeWidgetDescription, Description) 
    SELECT AcmeWidgetId, AcmeWidgetName, AcmeWidgetDescription, Description FROM @data;
";
    }
}
