using System;

namespace DemoDb.Application
{
    public class Widget
    {
        public Widget() { }

        public Widget(int widgetId, Guid globalId, string name, string description)
        {
            WidgetId = widgetId;
            GlobalId = globalId;
            Name = name;
            Description = description;
        }

        public Widget(Guid globalId, string name, string description) 
            : this(0, globalId, name, description)
        { }

        public int WidgetId { get; set; }
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
