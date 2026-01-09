using FabulaUltimaGMTool;
using FirstProject.Beastiary;
using FirstProject.Messaging;
using Godot;
using System;
using System.Threading.Tasks;

public partial class BattleMusicPlayer : AudioStreamPlayer
{
    [Export]
    public AudioStream VictoryMusicStream { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<EncounterLog>(this.ReceiveMessage);

        var runState = GetNode<RunState>("/root/RunState");       
        runState.VolumeLevelChanged += HandleVolumeChanged;
        var userConfiguration = GetNode<UserConfigurationState>("/root/UserConfigurationState").UserConfigurationData;
        
        var musicPath = runState.RunningEncounter.MusicFilePath;
        AudioStream stream = Stream;
        if (FileAccess.FileExists(musicPath))
        {
            var oggStream = AudioStreamOggVorbis.LoadFromFile(musicPath);
            oggStream.Loop = true;
            stream = oggStream;
        }

        Stream = stream;
        Playing = userConfiguration.BackgroundMusicEnabled;
        TreeExited += () => runState.VolumeLevelChanged -= HandleVolumeChanged;

        if(FileAccess.FileExists(runState.RunningEncounter.VictoryMusicFilePath))
        {
            VictoryMusicStream = AudioStreamOggVorbis.LoadFromFile(runState.RunningEncounter.VictoryMusicFilePath);
        }
    }

    private Task ReceiveMessage(IMessage message)
    {
        if (!(message is IMessage<EncounterLog> typedMessage)) return Task.CompletedTask;
        var action = typedMessage.Value;
        CallDeferred(MethodName.HandleLog, new SignalWrapper<EncounterLog>(action));
        return Task.CompletedTask;
    }

    public void HandleLog(SignalWrapper<EncounterLog> signal)
    {
        var action = signal.Value;
        if (!Playing) return;
        if (action.DisplayLevel != DisplayLevel.CELEBRATE) return;
        Playing = false;
        Stream = VictoryMusicStream;
        Playing = true;        
    }

    private void HandleVolumeChanged(double newVolumeLevel)
    {
        if (newVolumeLevel < 0.1)
        {
            VolumeDb = -80;
            return;
        }

        var newDb = Mathf.LinearToDb(newVolumeLevel);
        VolumeDb = (float)newDb;
    }
}
