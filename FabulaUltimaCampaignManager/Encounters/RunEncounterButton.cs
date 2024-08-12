
using FirstProject.Encounters;
using Godot;

public partial class RunEncounterButton : Button, IInitiativeSeedReader
{
	private InitiativeSeed _seed;
    private PackedScene _targetScene;
     // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnInitiativeSeedReady(InitiativeSeed seed)
    {
		seed.Changed += SeedChanged;
		_seed = seed;
    }

    private void SeedChanged()
    {
        if (_targetScene == null) return;
        if (!_seed.IsValid) return;
		this.Disabled = false;
    }

    internal void OnTargetSceneReady(PackedScene scene)
    {
        _targetScene = scene;
    }
}
