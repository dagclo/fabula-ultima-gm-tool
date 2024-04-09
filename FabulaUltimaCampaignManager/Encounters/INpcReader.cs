using FirstProject.Npc;

namespace FirstProject.Encounters
{
    public interface INpcReader
    {
        void HandleNpcChanged(NpcInstance npc);
    }
}
