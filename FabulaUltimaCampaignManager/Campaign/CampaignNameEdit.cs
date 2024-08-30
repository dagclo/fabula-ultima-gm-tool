using FirstProject.Beastiary;
using FirstProject.Campaign;
using Godot;

public partial class CampaignNameEdit : LineEdit
{   
    private CampaignData _campaign;
    
    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        _campaign = signal.Value;        
        this.Text = _campaign.Name;
    }

    public void OnTextChanged(string newText)
    {       
        if (_campaign == null) return;
        _campaign.Name = newText;
    }	
}
