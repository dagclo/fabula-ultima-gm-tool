using FirstProject.Campaign;
using Godot;
using System;

public partial class CampaignEntryPlayerNameContainer : RichTextLabel
{
    public void HandleCampaignChanged(CampaignData data)
    {
        Clear();
        AppendText("[b]Players[/b]:");
        Newline();
        foreach(var player in data.Players)
        {   
            AppendText($"[img=10%]{player.PortraitFile}[/img]{player.CharacterName}");
            AppendText($"[b]{player.CharacterName}[/b]");
            AppendText($" ({player.Name})");
        }
    }
}
