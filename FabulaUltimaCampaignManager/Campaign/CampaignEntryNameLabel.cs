using FirstProject.Campaign;
using Godot;
using System;

public partial class CampaignEntryNameLabel : RichTextLabel
{
    public void HandleCampaignChanged(CampaignData data)
    {
        this.Text = $"[b]Name: [/b] {data.Name}";
    }
}
