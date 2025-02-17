using Godot;
using System;

public partial class FindMusicButton : Button
{
	[Export]
	public FileDialog MusicFileDialog {  get; set; }

	public void HandlePressed()
	{
        MusicFileDialog.Visible = true;
    }
}
