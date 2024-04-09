using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;

public partial class UseAttackButton : Button, IAttackReader, INpcReader
{
	private BasicAttackTemplate _attack;
    private MessagePublisher<NpcActionMessage> _messagePublisher;
    private string _instanceName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
		_messagePublisher = messageRouter.GetPublisher<NpcActionMessage>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadAttack(BasicAttackTemplate attack)
    {
        _attack = attack;
    }

	public void OnUseAttack()
	{
        _messagePublisher.Publish((new NpcActionMessage
        {
            Id = _attack.Id,
            Action = _attack.Name,
            Actor = _instanceName,
            Verb = "uses",
        }).AsMessage());
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instanceName = npc.InstanceName;
    }
}
