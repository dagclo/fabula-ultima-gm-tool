using FabulaUltimaNpc;
using System;

public interface IBeastAttribute
{
    void HandleBeastChanged(IBeastTemplate beastTemplate);
    Action Save { get; set; }
}