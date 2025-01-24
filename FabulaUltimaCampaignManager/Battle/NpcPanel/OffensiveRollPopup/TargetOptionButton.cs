using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class TargetOptionButton : OptionButton
{
    private ICheckModel _checkModel;

    public override void _Ready()
    {
        var index = 0;        
        var players = GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid);
        foreach(var player in players)
        {
            this.AddItem($"{player.CharacterName} ({player.Name})", index++);
        }
        AddItem($"====NPCs====", index);
        SetItemDisabled(index, true);
        this.Selected = -1;
    }

    public void HandleReset()
    {
        this.Selected = -1;
    }

    public void HandleTargetListUpdate(Godot.Collections.Array<NpcInstance> npcs)
    {
        foreach(var  npc in npcs)
        {
            this.AddItem(npc.InstanceName);
        }
    }

    public void HandleCheckModelSet(SignalWrapper<ICheckModel> signal)
    {
        _checkModel = signal.Value;
    }

    public void HandleSelected(int index)
    {
        var target = this.GetItemText(index);
        _checkModel.Target = target;
    }
}
