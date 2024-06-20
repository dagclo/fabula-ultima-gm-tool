using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;

public partial class StudyInfo : VBoxContainer
{
    [Signal]
    public delegate void StudyLevelChangedEventHandler(SignalWrapper<BattleStatus.StudyLevelEnum> studyLevel);

    [Signal]
    public delegate void NpcChangedEventHandler(NpcInstance npc);

    public void SetStudyLevel(BattleStatus.StudyLevelEnum studyLevel)
    {
        EmitSignal(SignalName.StudyLevelChanged, new SignalWrapper<BattleStatus.StudyLevelEnum>(studyLevel));
    }

    public void SetNpc(NpcInstance npcInstance)
    {
        EmitSignal(SignalName.NpcChanged, npcInstance);
    }
}
