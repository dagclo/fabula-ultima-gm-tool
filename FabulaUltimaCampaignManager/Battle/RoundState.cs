using FirstProject.Npc;

public struct RoundState
{
    public int RoundNumber { get; set; }    

	public ITurnOwner CurrentTurnOwner { get; set; }
    public int TurnNumber { get; internal set; }
}

