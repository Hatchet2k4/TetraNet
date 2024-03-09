using Godot;
using System;

public partial class PlayerData : Node
{
	[Export] public string PlayerName;
	[Export] public string Team;

	public override void _Ready()
	{
		SetProcess(false);
	}
}
