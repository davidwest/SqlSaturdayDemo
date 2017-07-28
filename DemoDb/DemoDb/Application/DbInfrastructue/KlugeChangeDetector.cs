using System.Collections.Generic;
using System.Linq;

namespace DemoDb.Application.DbInfrastructue
{
    internal class KlugeChanges
    {
        public List<int> AddedRobotIds { get; set; }

        public List<int> RemovedRobotIds { get; set; }

        public List<Contraption> AddedContraptions { get; set; }
        
        public List<int> RemovedContraptionIds { get; set; }

        public Dictionary<int, List<int>> WidgetsAddedToExistingContraptions { get; set; }

        public Dictionary<int, List<int>> WidgetsRemovedFromExistingContraptions { get; set; }
    }
    
    internal class KlugeChangeDetector
    {
        public KlugeChanges DetectChanges(Kluge orig, Kluge updated)
        {
            var result = new KlugeChanges
            {
                AddedContraptions = 
                    updated.Contraptions
                    .Where(c => c.ContraptionId == default(int))
                    .ToList(),
                RemovedContraptionIds = 
                    orig.Contraptions
                    .Select(c => c.ContraptionId)
                    .Except(updated.Contraptions
                            .Select(c => c.ContraptionId))
                    .ToList(),
                AddedRobotIds = 
                    updated.RobotIds
                    .Except(orig.RobotIds)
                    .ToList(),
                RemovedRobotIds = 
                    orig.RobotIds
                    .Except(updated.RobotIds)
                    .ToList(),
                WidgetsAddedToExistingContraptions = new Dictionary<int, List<int>>(),
                WidgetsRemovedFromExistingContraptions = new Dictionary<int, List<int>>()
            };

            var existingContraptions =
                updated.Contraptions
                .Where(c => c.ContraptionId != default(int))
                .ToList();

            foreach (var existingContraption in existingContraptions)
            {
                var contraptionId = existingContraption.ContraptionId;

                var origContraption = 
                    orig.Contraptions
                    .Single(c => c.ContraptionId == existingContraption.ContraptionId);
                
                var addedWidgetIds = 
                    existingContraption.WidgetIds
                    .Except(origContraption.WidgetIds)
                    .ToList();

                if (addedWidgetIds.Count > 0)
                {
                    result.WidgetsAddedToExistingContraptions[contraptionId] = addedWidgetIds;
                }
                
                var removedWidgetIds =
                    origContraption.WidgetIds
                    .Except(existingContraption.WidgetIds)
                    .ToList();

                if (removedWidgetIds.Count > 0)
                {
                    result.WidgetsRemovedFromExistingContraptions[contraptionId] = removedWidgetIds;
                }
            }
            
            return result;
        }
    }
}
