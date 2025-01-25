using System;

namespace FabulaUltimaGMTool.Battle.NpcPanel
{
    public static class RollExtensions
    {
        public static EncounterLog ToEncounterLog(this CheckResult result, string action, string actorName, string damageType = "", string verb = "rolled")
        { 
            
            var successText = result.Success ? "Successful" : "Failed";
            var highRoll = result.Success ? $"[hr+mod]=[{result.HighRoll}+{result.HighRollMod}]={result.FinalHighRoll} {damageType}" : string.Empty;
            var detailString = $"[{result.Attribute1Name}+{result.Attribute2Name}+mod]=[{result.Attribute1Result}+{result.Attribute2Result}+{result.ResultMod}]=[{result.TotalRoll}]";
            var targetString = string.IsNullOrWhiteSpace(result.Target) ? string.Empty : $"Target: {result.Target}";
            return new EncounterLog
            {
                Id = Guid.NewGuid(),
                Action = $"{action} | {targetString} | Result: {successText}|{detailString}|{highRoll}",
                Actor = actorName,
                Verb = verb,
                Object = string.IsNullOrWhiteSpace(result.Target) ? string.Empty : result.Target,
                DisplayLevel = result.Success ? DisplayLevel.SUCCESS : DisplayLevel.FAILED
            };
        }
    }
}
