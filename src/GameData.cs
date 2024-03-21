namespace TetraNet;

using Godot;
using System.Collections.Generic;

public class ChatEntry
{
	public long Id { get; set; }
	public string Text { get; set; }
}



public partial class GameData : Node
{
	public Dictionary<long, PlayerData> PlayerList = new();

	public Dictionary<long, int> fieldMappings = new();

	public string PlayerName;
	public string Team;
	public long Id;

	public List<ChatEntry> ChatLog = new();

	public override void _Ready()
	{

	}

	public string GetPlayerName(int index)
	{
		return PlayerList[index].PlayerName;
	}

	public string GetPlayerName(long id)
	{
		return PlayerList[fieldMappings[id]].PlayerName;
	}

	public void AddChat(long id, string text)
	{
		ChatEntry chat = new()
		{
			Id = id,
			Text = text
		};
		ChatLog.Add(chat);
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
		foreach (long _id in PlayerList.Keys) GD.Print("Playerlist: " + _id.ToString());
	}

	public void RemovePlayer(long id)
	{
		PlayerList.Remove(id);
	}
}
