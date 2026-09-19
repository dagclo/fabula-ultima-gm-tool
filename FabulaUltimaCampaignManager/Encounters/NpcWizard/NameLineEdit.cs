using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NameLineEdit : LineEdit, INpcReader
{
    private NpcInstance _instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.FocusExited += () => OnSubmit(this.Text); // commit on click-away, not just Enter
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
		Text = npc.InstanceName;
    }

	public void OnSubmit(string newText)
	{
		if (_instance == null) return;
		_instance.InstanceName = newText;
	}
}
