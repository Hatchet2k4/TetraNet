namespace TetraNet;

using Godot;
using System;

public partial class Block : Node2D
{
	private readonly int min_x = -54;
	private readonly int max_x = 54;

	private Resource pieceScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()	
	{
		pieceScene = GD.Load("res://scenes");
		SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
