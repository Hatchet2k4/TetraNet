namespace TetraNet;

using Godot;


public partial class Title : Node2D
{
	[Export] private AudioStreamPlayer titleMusic;
	[Export] private BoxContainer mainMenu;
	[Export] private Control menuBox;

	public override void _Process(double delta)
	{
		if (!menuBox.Visible && (Gamepad.PressedStart() || Gamepad.PressedA()) || Input.IsMouseButtonPressed(MouseButton.Left))
		{
			menuBox.Visible = true;
		}
	}

	public void SoloGame()
	{
		titleMusic.Stop();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	public void StartServer()
	{

	}

	public void ConnectToServer()
	{

	}

	public void Options()
	{

	}

	public void SetupControls()
	{

	}

}
