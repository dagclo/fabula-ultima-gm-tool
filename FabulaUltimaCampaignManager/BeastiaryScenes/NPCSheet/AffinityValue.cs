using Godot;

public partial class AffinityValue : OptionButton
{

    [Signal]
    public delegate void AffinitySelectedEventHandler(string affinityValue);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0);
        foreach (var val in new[] { "", "RS", "IM", "AB", "VU" })
        {
            AddItem(val);
        }
    }

    public void HandleUpdateAffinity(string affinity)
    {

        if (affinity == string.Empty)
        {
            CallDeferred(MethodName.SetDisabled, false);
        }
        for(var index = 0; index < this.ItemCount; index++)
        {            
            string itemName = this.GetItemText(index);
            if (itemName == affinity)
            {   
                CallDeferred(MethodName.Select, index);
                if(affinity != string.Empty) CallDeferred(MethodName.SetDisabled, true);
                break;
            }
        }        
    }

    public void HandleItemSelected(int index)
    {
        var value = this.GetItemText(index);
        EmitSignal(SignalName.AffinitySelected, value);
    }
}
