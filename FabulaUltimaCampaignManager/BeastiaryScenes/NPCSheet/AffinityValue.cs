using Godot;

public partial class AffinityValue : OptionButton
{
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
        if (affinity == string.Empty) return;
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
}
