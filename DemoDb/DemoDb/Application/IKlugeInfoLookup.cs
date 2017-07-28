using System.Collections.Generic;

namespace DemoDb.Application
{
    public interface IKlugeInfoLookup
    {
        IReadOnlyList<KlugeInfo> GetKluges(string search = null);

        KlugeInfo GetKluge(int klugeId);
    }
}
