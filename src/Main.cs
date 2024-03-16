namespace TetraNet;

using Godot;

public partial class Main : Control
{
	[Export] public Camera2D Camera;
	[Export] public MainField mainField;
	public ConnectionHandler connection;
	public GameData gameData;
	public Title title;

	public override void _Ready()
	{
		SetProcess(false);
	}

	public override void _Process(double delta)
	{
	}

	public void SetName(string name)
	{
		mainField.SetName(name);
	}

	public void Start()
	{
		Camera.Enabled = true;
		Show();
		mainField.NewGame();
	}

	public void StopGame()
	{
		title.ReturnFromGame();
	}
}
