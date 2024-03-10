using Godot;
using System;

public struct PlayerData
{
	[Export] public string PlayerName { get; set; }
	[Export] public string Team { get; set; }
}
