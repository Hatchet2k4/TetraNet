namespace TetraNet;

using Godot;


public partial class Title : Node2D
{
	[Export] private AudioStreamPlayer titleMusic;
	[Export] private Control mainMenu;
	[Export] private Control hostMenu;
	[Export] private Control joinMenu;

	private bool started=false;

	public override void _Process(double delta)
	{
		if (!started && !mainMenu.Visible && (Gamepad.PressedStart() || Gamepad.PressedA()) || Input.IsMouseButtonPressed(MouseButton.Left))
		{
			mainMenu.Show();
			started=true;
		}
	}

	public void SoloGame()
	{
		titleMusic.Stop();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	public void StartServer()
	{
		mainMenu.Hide();
		hostMenu.Show();
	}

	public void ConnectToServer()
	{
		mainMenu.Hide();
		joinMenu.Show();
	}

	public void Options()
	{

	}

	public void SetupControls()
	{

	}

}
