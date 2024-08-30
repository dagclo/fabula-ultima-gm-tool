using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NpcAttributeLabel : PanelContainer, INpcReader, INpcStatusReader
{
	[Export]
	public string Attribute {  get; set; }

	private Label Value;
    private NpcInstance _npcInstance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Value = (Label) FindChild("Text"); // force exception if not found
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npcInstance = npc;
        Value.Text = _npcInstance.GetValueOf(Attribute).ToString();
    }

    public void HandleStatusSet(BattleStatus status)
    {
        status.StatusChanged += UpdateAttribute;
    }

    private void UpdateAttribute(BattleStatus status)
    {
        var currentValue = _npcInstance.GetValueOf(Attribute);
        var postStatus = status.ApplyStatus(Attribute, _npcInstance);
        Value.Text = postStatus.ToString();
    }
}
