using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public partial class DamageTypeOptions : OptionButton, INpcReader
{
    [Signal]
    public delegate void DamageTypeAndAffinityChangedEventHandler(SignalWrapper<Affinity> affinitySignal, string damageType);


    private readonly IDictionary<int, string> _indexToDamageNameMap = new Dictionary<int, string>();
    private IReadOnlyDictionary<string, Affinity> _affinities;
    public const string HEAL = "Heal";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
		var repository = dBAccess.Repository;
        var damageTypeValues = repository.GetDamageTypes().Where(v => !string.Equals(v.Name, "no damage", StringComparison.InvariantCultureIgnoreCase ));
		var startIndex = this.ItemCount;
		foreach(var name in damageTypeValues)
		{
			this.AddItem(name.Name, startIndex);
			_indexToDamageNameMap[startIndex] = name.Name;
            startIndex++;
		}
		
        _indexToDamageNameMap[startIndex] = HEAL;
        this.AddItem(HEAL, startIndex);
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		_affinities = npc.Resistances;
        var damageNameToIndexMap = _indexToDamageNameMap.ToDictionary(p => p.Value, p => p.Key);
        foreach(var affinity in _affinities)
        {
            var itemIndex = damageNameToIndexMap[affinity.Key];
            var itemText = GetItemText(itemIndex);
            var newText = affinity.Value != Affinity.NONE ? $"{itemText} - {affinity.Value}" : itemText;
            SetItemText(itemIndex, newText);
        }
    }

    public void OnItemSelected(int index)
    {
        if (!_indexToDamageNameMap.ContainsKey(index)) return;
        var damageType = _indexToDamageNameMap[index];        
        Affinity affinity;        
        
        if (damageType == HEAL)
        {
            affinity = Affinity.HEAL;            
        }
        else
        {
            affinity = _affinities[damageType];            
        }

        EmitSignal(SignalName.DamageTypeAndAffinityChanged, new SignalWrapper<Affinity> (affinity), damageType);
    }
}
