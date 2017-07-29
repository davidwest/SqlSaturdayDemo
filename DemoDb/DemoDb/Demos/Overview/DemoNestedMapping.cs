using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluidDbClient;

namespace DemoDb.Demos.Overview
{
    public class Thing
    {
        public Thing(int id, string name, List<Module> modules)
        {
            Id = id;
            Name = name;
            Modules = modules;
        }

        public int Id { get; }
        public string Name { get; }

        public List<Module> Modules { get; }
    }

    public class Module
    {
        public Module(int id, string name, List<Component> components)
        {
            Id = id;
            Name = name;
            Components = components;
        }

        public int Id { get; }
        public string Name { get; }

        public List<Component> Components { get; } 
    }

    public class Component
    {
        public Component(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }

    public class ThingDataRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Module_Id { get; set; }
        public string Module_Name { get; set; }
        public int Component_Id { get; set; }
        public string Component_Name { get; set; }        
    }
    
    public static class DemoNestedMapping
    {
        public static void Start()
        {
            /*
                SELECT t.Id, t.Name, m.Id AS Module_Id, m.Name AS Module_Name, c.Id AS Component_Id, c.Name AS Component_Name
                FROM Thing AS t
                LEFT JOIN Module AS m ON t.Id = m.ThingId
                LEFT JOIN Component AS c ON m.Id = c.ModuleId
            */
            var records = new[]
            {
                new ThingDataRecord{Id = 1, Name = "Thing1", Module_Id = 1, Module_Name = "Module1", Component_Id= 1, Component_Name="Component1"},
                new ThingDataRecord{Id = 1, Name = "Thing1", Module_Id = 1, Module_Name = "Module1", Component_Id= 2, Component_Name="Component2"},
                new ThingDataRecord{Id = 1, Name = "Thing1", Module_Id = 2, Module_Name = "Module2", Component_Id= 3, Component_Name="Component3"},
                new ThingDataRecord{Id = 1, Name = "Thing1", Module_Id = 2, Module_Name = "Module2", Component_Id= 4, Component_Name="Component4"},
                new ThingDataRecord{Id = 2, Name = "Thing2", Module_Id = 3, Module_Name = "Module3", Component_Id= 5, Component_Name="Component5"},
                new ThingDataRecord{Id = 2, Name = "Thing2", Module_Id = 3, Module_Name = "Module3", Component_Id= 6, Component_Name="Component6"},
                new ThingDataRecord{Id = 2, Name = "Thing2", Module_Id = 4, Module_Name = "Module4", Component_Id= 7, Component_Name="Component7"},
                new ThingDataRecord{Id = 2, Name = "Thing2", Module_Id = 4, Module_Name = "Module4", Component_Id= 8, Component_Name="Component8"},
            };

            var things =
                records.MapNested(rec => rec.Id,
                                  rec => rec.Module_Id,
                                  rec => rec.Component_Id,
                                  (rec, modules) => new Thing(rec.Id, rec.Name, modules.ToList()),
                                  (rec, components) => new Module(rec.Module_Id, rec.Module_Name, components.ToList()),
                                  rec => new Component(rec.Component_Id, rec.Component_Name));

            foreach (var thing in things)
            {
                Debug.WriteLine($"{thing.Id, -6} {thing.Name}");

                foreach (var mod in thing.Modules)
                {
                    Debug.WriteLine($"     {mod.Id, -6} {mod.Name}");

                    foreach (var comp in mod.Components)
                    {
                        Debug.WriteLine($"          {comp.Id,-6} {comp.Name}");
                    }
                }
            }
        }
    }
}
