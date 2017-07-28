using System.Collections.Generic;

namespace DemoDb.Application
{
    // --- command ---

    public class Kluge
    {
        public int KlugeId { get; set; }

        public string Name { get; set; }
        
        public List<int> RobotIds { get; set; }
        
        public List<Contraption> Contraptions { get; set; }
    }

    public class Contraption
    {
        public int ContraptionId { get; set; }

        public string Name { get; set; }

        public List<int> WidgetIds { get; set; }
    }


    // --- query ---

    public class KlugeInfo
    {
        public int KlugeId { get; set; }

        public string Name { get; set; }

        public List<Robot> Robots { get; set; }
        
        public List<ContraptionInfo> Contraptions { get; set; }
    }
    
    public class ContraptionInfo
    {
        public int ContraptionId { get; set; }

        public string Name { get; set; }

        public List<Widget> Widgets { get; set; }
    }
}
