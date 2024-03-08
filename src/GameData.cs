namespace TetraNet;

using Godot;
using System;

public partial class GameData : Node
{
	[Export] public Godot.Collections.Dictionary<long, PlayerInfo> AllPlayers = new();


}
