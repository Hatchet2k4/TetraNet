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

	public void UpdatePlayer(long id, string name, string team)
	{
		var pd = PlayerList[id];
		pd.PlayerName = name;
		pd.Team = team;
		PlayerList[id] = pd;
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
