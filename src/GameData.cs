namespace TetraNet;

using Godot;
using System.Linq;

public partial class GameData : Node
{
	public Godot.Collections.Dictionary<long, string> PlayerList = new();
	[Export] Node playerNode;


	public override void _Ready()
	{

	}

	public void AddPlayer(long id, string name)
	{
		PlayerList[id] = name;
		//Spawner.Spawn(p);
		//AddChild(p);
		//RefreshPlayerList();
	}

	public void RemovePlayer(long id)
	{
		PlayerList.Remove(id);
		/*
		try
		{
			Node p = playerNode.FindChild(id.ToString());
			p.QueueFree();
		}
		catch { }*/
		//RefreshPlayerList();
	}

	public void RefreshPlayerList()
	{
		PlayerList.Clear();

		System.Collections.Generic.IEnumerable<PlayerData> playerNodes = playerNode.GetChildren()
			.Where(child => child is PlayerData)
			.Select(child => child)
			.Cast<PlayerData>();

		foreach (PlayerData p in playerNodes)
		{
			PlayerList[long.Parse(p.Name)] = p.PlayerName;
		}
	}
}
