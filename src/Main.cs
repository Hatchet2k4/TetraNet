namespace TetraNet;

using Godot;

public partial class Main : Control
{
	[Export] public ConnectionHandler Connection;
	[Export] public Camera2D Camera;
	[Export] public GameData GameData;
	[Export] public MainField mainField;

	public override void _Ready()
	{
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


}
