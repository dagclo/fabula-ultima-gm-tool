using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class ShowStats : Button, INpcReader
{
	[Export]
	public PackedScene StatWindowScene { get; set; }

    private NpcStatWindow _statWindow { get; set; }

    private NpcInstance _npcInstance { get; set; }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npcInstance = npc;
    }

    public void OnPressed()
	{
        _statWindow = StatWindowScene.Instantiate<NpcStatWindow>();
        _statWindow.OnClose += OnClose;
        AddChild(_statWindow);
        _statWindow.SetBeast(_npcInstance.Template);
        _statWindow.Show();
        this.Disabled = true;
    }

    private void OnClose()
    {        
        _statWindow.OnClose -= OnClose;
        RemoveChild(_statWindow);
        _statWindow.QueueFree();
        _statWindow = null;
        this.Disabled = false;
    }
}
