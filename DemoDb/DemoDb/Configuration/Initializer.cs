using System.Transactions;
using DemoDb.Application;
using DemoDb.Application.DbInfrastructue;
using FluidDbClient;
using FluidDbClient.Sql;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DemoDb
{
    public static class Initializer
    {
        public static void Initialize()
        {
            InitializeDb();
            InitializeServices();
        }

        private static void InitializeDb()
        {
            const string acmeConnStr = "Server = localhost; Initial Catalog = Acme; Trusted_Connection = true;";
            const string bubbleWrapConnStr = "Server = localhost; Initial Catalog = BubbleWrapCorp; Trusted_Connection = true;";

            DbRegistry.Initialize(new AcmeDb(acmeConnStr), new BubbleWrapDb(bubbleWrapConnStr));

            TableTypeRegistry.Register<AcmeDb>(new RobotsTableTypeMap(), new WidgetsTableTypeMap());
            TableTypeRegistry.Register<BubbleWrapDb>(new WidgetSleeveTableTypeMap());
        }

        private static void InitializeServices()
        {
            var container = new Container();
            
            container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            container.RegisterSingleton<IWidgetLookup, DbWidgetLookup>();
            container.RegisterSingleton<IRobotLookup, DbRobotLookup>();

            var txOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            container.Register(() => new TransactionScope(TransactionScopeOption.Required, txOptions), Lifestyle.Scoped);
            container.Register(() => new DbSession(), Lifestyle.Scoped);

            container.Register<ISaveNewKlugeCommandHandler, DbSaveNewKlugeCommandHandler1>(Lifestyle.Scoped);
            container.Register<ISaveUpdatedKlugeCommandHandler, DbSaveUpdatedKlugeCommandHandler>(Lifestyle.Scoped);

            container.RegisterSingleton<IKlugeInfoLookup, DbKlugeInfoLookup>();

            container.Verify();

            AppServices.Initialize(container);
        }
    }
}
