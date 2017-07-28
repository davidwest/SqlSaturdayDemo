using System.Collections.Generic;

namespace DemoDb.Application
{
    public interface IRobotLookup
    {
        IReadOnlyList<Robot> GetRobots(string search = null);
    }
}
