namespace TetraNet;

using Godot;

public partial class Title : Node2D
{
	[Export] private AudioStreamPlayer titleMusic;
	[Export] private Control mainMenu;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private GameData _gameData;

	private bool started = false;
	private double timer;

	public override void _Ready()
	{
		ConfigData.Load();
		_gameData.PlayerName = ConfigData.PlayerName;
	}

	public override void _Process(double delta)
	{
		timer += delta;
		if (!started && !mainMenu.Visible && (timer > 2f || Gamepad.PressedStart() || Gamepad.PressedA() || Input.IsMouseButtonPressed(MouseButton.Left)))
		{
			mainMenu.Show();
			started = true;
			SetProcess(false);
		}

	}

	public void ReturnFromSubMenu()
	{
		hostJoinMenu.Hide();
		mainMenu.Show();
	}

	public void SoloGame()
	{
		titleMusic.Stop();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	public void StartServer()
	{
		mainMenu.Hide();
		hostJoinMenu.SetMode(ConnectionMode.Host);
		hostJoinMenu.Show();
	}

	public void ConnectToServer()
	{
		mainMenu.Hide();
		hostJoinMenu.SetMode(ConnectionMode.Client);
		hostJoinMenu.Show();
	}

	public void Options()
	{

	}

	public void SetupControls()
	{

	}

}
