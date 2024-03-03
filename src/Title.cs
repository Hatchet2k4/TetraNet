namespace TetraNet;

using Godot;


public partial class Title : Node2D
{
	[Export] private AudioStreamPlayer titleMusic;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Gamepad.PressedStart() || Gamepad.PressedA())
		{
			titleMusic.Stop();
			GetTree().ChangeSceneToFile("res://scenes/main.tscn");
		}
	}
}
