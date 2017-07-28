using System;

namespace DemoDb.Application
{
    public class Robot
    {
        public Robot() { }

        public Robot(int robotId, string name, string description, DateTime dateBuilt, DateTime? dateDestroyed, bool isEvil)
        {
            RobotId = robotId;
            Name = name;
            Description = description;
            DateBuilt = dateBuilt;
            DateDestroyed = dateDestroyed;
            IsEvil = isEvil;
        }

        public Robot(string name, string description, DateTime dateBuilt, bool isEvil) 
            : this(0, name, description, dateBuilt, null, isEvil)
        { }

        public int RobotId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateBuilt { get; set; }
        public DateTime? DateDestroyed { get; set; }
        public bool IsEvil { get; set; }
    }
}
