using FabulaUltimaNpc;
using System;

public interface IBeastAttribute
{
    void HandleBeastChanged(IBeastTemplate beastTemplate);
    Action<bool> Save { get; set; }
}