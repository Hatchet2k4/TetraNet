namespace TetraNet;

using Godot;
using System;

public partial class Gradient : TextureRect
{
	double timer;
	double fadeTime = 0.25f;

	public override void _Ready()
	{
		timer = fadeTime;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer -= delta;
		if (timer > 0f)
		{
			float percent = (float)(timer / fadeTime);
			Modulate = new Color(1f, 1f, 1f, percent);
		}
		else
		{
			QueueFree();
		}
	}
}
