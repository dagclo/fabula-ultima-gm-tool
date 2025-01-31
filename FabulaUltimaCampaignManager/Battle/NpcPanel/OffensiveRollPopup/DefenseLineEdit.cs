using FirstProject.Beastiary;
using Godot;

public partial class DefenseLineEdit : LineEdit
{
    private ICheckModel _checkModel;

    public void HandleCheckModelSet(SignalWrapper<ICheckModel> signal, BattleStatus status)
    {
        _checkModel = signal.Value;
    }

    public void HandleTextChanged(string newText)
    {
        if(!int.TryParse(newText, out var defense))
        {
            return;
        }
        _checkModel.Difficulty = defense;
    }


    public void HandleReset()
    {
        this.Text = string.Empty;
    }
}
