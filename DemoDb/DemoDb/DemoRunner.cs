using DemoDb.Demos;
using DemoDb.Demos.App;
using DemoDb.Demos.Overview;

namespace DemoDb
{
    public static class DemoRunner
    {
        public static void Start()
        {
            // --- OVERVIEW ---
            
            DemoBasics.Start();
            DemoMultiDbOperation.Start();
            DemoToStructuredData.Start();
            DemoToStructuredDataWithTypeName.Start();
            DemoCombineStructuredDataAndMultiParam.Start();
            
            // --- APP ---

            //UserCreatesNewKluges.Start();
            //UserViewsAllKluges.Start();

            //UserUpdatesKluge.Start();
            //UserViewsAllKluges.Start();
        }
    }
}
