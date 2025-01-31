using FirstProject.Npc;
using System.Collections.Generic;

namespace FabulaUltimaGMTool.Battle
{
    public struct EncounterInitialize
    {   
        public ICollection<(NpcInstance npc, BattleStatus status)> Npcs { get; init; }
    }
}
