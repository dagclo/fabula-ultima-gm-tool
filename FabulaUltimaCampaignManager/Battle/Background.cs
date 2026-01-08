using FabulaUltimaGMTool;
using Godot;

public partial class Background : AnimatedSprite2D
{
    [Export]
    public AudioStreamPlayer AudioPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var runState = GetNode<RunState>("/root/RunState");
        var targetAnimation = runState.RunningEncounter.Background.ToString();
        runState.VolumeLevelChanged += HandleVolumeChanged;
        
        var userConfiguration = GetNode<UserConfigurationState>("/root/UserConfigurationState").UserConfigurationData;
        this.Animation = targetAnimation;

        var musicPath = runState.RunningEncounter.MusicFilePath;
        AudioStream stream = AudioPlayer.Stream;
        if (FileAccess.FileExists(musicPath))
        {
            var oggStream = AudioStreamOggVorbis.LoadFromFile(musicPath);
            oggStream.Loop = true;
            stream = oggStream;
        }       

        AudioPlayer.Stream = stream;
        AudioPlayer.Playing = userConfiguration.BackgroundMusicEnabled;
        AudioPlayer.TreeExited += () => runState.VolumeLevelChanged -= HandleVolumeChanged;
    }

    private void HandleVolumeChanged(double newVolumeLevel)
    {
        if(newVolumeLevel < 0.1)
        {            
            AudioPlayer.VolumeDb = -80;
            return;
        }

        var newDb = Mathf.LinearToDb(newVolumeLevel);
        AudioPlayer.VolumeDb = (float) newDb;
    }
}
