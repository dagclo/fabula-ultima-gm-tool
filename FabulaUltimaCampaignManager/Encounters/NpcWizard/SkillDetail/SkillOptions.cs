using FirstProject;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;
using FabulaUltimaSkillLibrary;
using FabulaUltimaNpc;
using System.Collections.Generic;
using FirstProject.Beastiary;

public partial class SkillOptions : OptionButton, INpcReader
{
    private NpcInstance _instance;
    [Signal]
    public delegate void SkillSelectedEventHandler(SignalWrapper<SkillTemplate> signalWrapper);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
    
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private IList<SkillTemplate> _skills;
    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
        if(_instance != null)
        {
            var dBAccess = GetNode<DbAccess>("/root/DbAccess");
            _skills = dBAccess.Repository.GetSkills().Where(s => s.SpeciesCanUse(_instance.Template)).ToList();
            foreach ((var skill, var index) in _skills.Select((s, i) => (s, i)))
            {
                AddItem(skill.Name, index);
            }
            this.Selected = -1;
        }
    }

    public void OnItemSelected(int index)
    {
        var id = GetItemId(index);
        var skill = _skills[id];
        EmitSignal(SignalName.SkillSelected, new SignalWrapper<SkillTemplate>(skill));
    }
}
