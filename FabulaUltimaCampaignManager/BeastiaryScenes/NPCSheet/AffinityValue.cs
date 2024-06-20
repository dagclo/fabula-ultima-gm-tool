using FabulaUltimaSkillLibrary;
using Godot;
using System;

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
        for(var index = 0; index < this.ItemCount; index++)
        {            
            string itemName = this.GetItemText(index);
            if (itemName == affinity)
            {
                this.Select(index);
                break;
            }
        }
        
    }
}
