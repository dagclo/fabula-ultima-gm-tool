using FirstProject.Npc;

public interface INpcInstanceReader
{
	int SlotIndex { get; set; }

	void ReadNpc(NpcInstance instance, BattleStatus battleStatus);
}
