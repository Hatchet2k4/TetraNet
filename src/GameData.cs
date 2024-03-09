namespace TetraNet;

using Godot;
using System;

public partial class GameData : Node
{
	public Godot.Collections.Dictionary<long, string> GetPlayerList()
	{
		Godot.Collections.Dictionary<long, string> playerList = new();



		return playerList;
	}
}
