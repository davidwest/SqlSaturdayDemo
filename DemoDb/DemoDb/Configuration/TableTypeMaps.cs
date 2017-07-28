using DemoDb.Application;
using FluidDbClient.Sql;

namespace DemoDb
{    
    public class WidgetsTableTypeMap : TableTypeMap<Widget>
    {
        public WidgetsTableTypeMap()
        {
            HasName("Widgets");

            Property(x => x.GlobalId)
                .IsInUniqueKey();

            Property(x => x.Name)
                .HasLength(100);

            Property(x => x.Description)
                .HasLength(500);
        }
    }

    public class RobotsTableTypeMap : TableTypeMap<Robot>
    {
        public RobotsTableTypeMap()
        {
            HasName("Robots");

            Property(x => x.Name)
                .HasLength(100);

            Property(x => x.Description)
                .HasLength(500);
        }
    }

    public class WidgetSleeveTableTypeMap : TableTypeMap<WidgetSleeve>
    {
        public WidgetSleeveTableTypeMap()
        {
            HasName("WidgetSleeves");

            Property(x => x.AcmeWidgetDescription)
                .HasBehavior(ColumnBehavior.Nullable)
                .HasLength(500);

            Property(x => x.Description)
                .HasLength(500);
        }
    }
}
