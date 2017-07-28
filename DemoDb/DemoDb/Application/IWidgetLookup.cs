using System.Collections.Generic;

namespace DemoDb.Application
{
    public interface IWidgetLookup
    {
        IReadOnlyList<Widget> GetWidgets();
    }
}
