using System;

namespace DemoDb.Demos.Overview
{
    public class PhoneNumber
    {
        public int AreaCode { get; set; }
        public int Prefix { get; set; }
        public int Postfix { get; set; }
    }
    
    public class Locale
    {
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }

    public class MailingAddress
    {
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }

        public Locale Locale { get; set; }
    }

    public class ContactInfo
    {
        public MailingAddress Address { get; set; }
        public PhoneNumber Phone { get; set; }
    }

    public class ImmutableRobot
    {
        public ImmutableRobot(int robotId, 
                              string name, 
                              string description, 
                              DateTime dateBuilt, 
                              DateTime? dateDestroyed,
                              bool isEvil)
        {
            RobotId = robotId;
            Name = name;
            Description = description;
            DateBuilt = dateBuilt;
            DateDestroyed = dateDestroyed;
            IsGood = !isEvil;
        }

        public int RobotId { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime DateBuilt { get; }
        public DateTime? DateDestroyed { get; }
        public bool IsGood { get;  }
    }
}
