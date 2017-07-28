using System.Diagnostics;
using DemoDb.Application;
using DemoDb.Demos.Overview;

namespace DemoDb.Demos.App
{
    public static class UserViewsAllKluges
    {
        public static void Start()
        {
            var lookup = AppServices.Get<IKlugeInfoLookup>();

            var kluges = lookup.GetKluges();

            foreach (var k in kluges)
            {
                Debug.WriteLine(k.ToDisplayString());
            }
        }
    }
}
