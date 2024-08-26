using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System.Linq;

public partial class ChooseDamageTypeOptions : OptionButton
{
    private BasicAttackTemplate _basicAttack;

    [Export]
    public Godot.Collections.Array<string> DamageTypes { get; set; } = new Godot.Collections.Array<string>(DamageConstants.DamageTypes);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach ((var val, int index) in DamageTypes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(-1);
    }

    public void OnSelected(int index)
    {
        if(_basicAttack == null) return;
        var damageTypeName = DamageTypes[GetItemId(index)];
        _basicAttack.DamageType = new FabulaUltimaNpc.DamageType
        {
            Id = DamageConstants.DamageTypeMap[damageTypeName],
            Name = damageTypeName,
        };        
    }

    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        _basicAttack = attack;
        if (_basicAttack.DamageType == null) return;
        var id = DamageTypes.IndexOf(_basicAttack.DamageType.Name.ToLowerInvariant());
        Select(GetItemIndex(id));
    }
}
