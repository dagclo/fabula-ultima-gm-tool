using FirstProject.Beastiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaGMTool.Battle.NpcPanel
{
    public static class RollExtensions
    {
        public static EncounterLog ToEncounterLog(this CheckResult result, string action, string actorName)
        { 
            
            var successText = result.Success ? "Successfully" : "Failed";
            var highRoll = result.Success ? $"[hr+mod]=[{result.HighRoll}+{result.HighRollMod}={result.FinalHighRoll}" : string.Empty;
            var detailString = $"[{result.Attribute1Name}+{result.Attribute2Name}+mod]=[{result.Attribute1Result}+{result.Attribute2Result}+{result.ResultMod}]=[{result.TotalRoll}]";
            return new EncounterLog
            {
                Id = Guid.NewGuid(),
                Action = $"{action} {successText}|{detailString}|{highRoll}",
                Actor = actorName,
                Verb = "rolled",
            };
        }
    }
}
