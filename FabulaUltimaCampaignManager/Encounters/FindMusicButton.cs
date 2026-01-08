using Godot;

public partial class FindMusicButton : Button
{
	[Export]
	public Godot.FileDialog MusicFileDialog {  get; set; }

	public void HandlePressed()
	{
        MusicFileDialog.Visible = true;
    }
}
