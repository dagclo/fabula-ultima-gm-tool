using FirstProject.Npc;

public class NpcTurnOwner : ITurnOwner
{
    public NpcInstance Instance { get; }

    public NpcTurnOwner(NpcInstance instance, BattleStatus status)
    {
        Instance = instance;
        BattleStatus = status;
    }

    public TurnOwnerKind TurnOwnerKind => TurnOwnerKind.NPC;

    public BattleStatus BattleStatus { get; }

    public string Name => Instance.InstanceName;

    public bool Match(object o)
    {
        return o.Equals(Instance);
    }
}
