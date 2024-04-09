using System;

public interface IPlayerTurnHandler : IPlayerStatus
{    
    void OnRoundStart();
    void OnTurnStart();
    void OnRoundEnd();

    Action CompletedTurn { get; set; }
}
