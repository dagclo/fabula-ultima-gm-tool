using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteConfirmButton : Button, IBeastAttribute
{
    private IBeastTemplate _beast;
    public Action<bool> Save { get; set; }
      
    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beast = beastTemplate;
    }

    public void OnPressed()
    {
        // use beast id to delete from database
        Save.Invoke(true);
    }
}
