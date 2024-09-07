using FirstProject.Encounters;
using System;

public interface IInitiativeSeedReader
{
    Action OnSubmit { get; set; }

    void OnInitiativeSeedReady(InitiativeSeed seed);
}
