using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class NpcInfo : HBoxContainer, INpcReader
{
    private NpcInstance _npc;
    private BattleStatus.StudyLevelEnum _studyLevel;

    [Export]
    public PackedScene ToolTip { get; set; }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npc = npc;
    }

    public void HandleStudyLevelChanged(BattleStatus newStatus)
    {
        _studyLevel = newStatus.StudyLevel;
    }

    public override Control _MakeCustomTooltip(string forText)
    {        
        var tooltip = ToolTip?.Instantiate<StudyInfo>();
        if (tooltip != null)
        {
            tooltip.SetNpc(_npc);
            tooltip.SetStudyLevel(_studyLevel);
        }        
        return tooltip;
    }
}
