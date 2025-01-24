using FirstProject.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaGMTool.Battle
{
    public struct EncounterInitialize
    {   
        public ICollection<(NpcInstance npc, BattleStatus status)> Npcs { get; init; }
    }
}
