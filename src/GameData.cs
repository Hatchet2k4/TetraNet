namespace TetraNet;

using Godot;
using System.Collections.Generic;

public partial class GameData : Node
{
	public Dictionary<long, PlayerData> PlayerList = new();

	public string PlayerName;
	public string Team;

	public override void _Ready()
	{

	}

	public void AddPlayer(long id, string name, string team)
	{
		PlayerData p = new()
		{
			PlayerName = name,
			Team = team
		};
		PlayerList[id] = p;
	}

	public void RemovePlayer(long id)
	{
		PlayerList.Remove(id);
	}
}
