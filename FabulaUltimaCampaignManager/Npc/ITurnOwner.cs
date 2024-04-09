namespace FirstProject.Npc
{
    public interface ITurnOwner
    {
        public TurnOwnerKind TurnOwnerKind { get; }

        string Name { get; }

        public bool Match(object o);
    }
}
