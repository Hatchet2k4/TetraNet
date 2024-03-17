using Godot;
using System;

public partial class Item : TextureRect
{
	[Export] private Label itemCount;
	public int count;
	public int maxCount;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
