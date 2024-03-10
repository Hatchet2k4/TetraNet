namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class Lobby : Control
{
	[Export] private Title _title;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private ConnectionHandler connection;
	[Export] public LineEdit _nameBox;
	[Export] public OptionButton _teamOption;
	[Export] private PackedScene _connectedRow;
	[Export] private Control _rowListNode;
	[Export] public GameData gameData;

	private int _maxRows = 12;
	private List<ConnectedRow> _rowList;
	public double refreshTimer;

	public string PlayerName
	{
		get
		{
			return connection.Name;
		}
		set
		{
			_nameBox.Text = value;
			connection.Name = value;
		}
	}

	public override void _Ready()
	{
		VisibilityChanged += VisibleChanged;
		_rowList = new();
		for (int i = 0; i < _maxRows; i++)
		{
			var row = _connectedRow.Instantiate() as ConnectedRow;
			_rowListNode.AddChild(row);
			row.Position = new Vector2(0, 24 * i);
			_rowList.Add(row);
		}
	}

	public override void _Process(double delta)
	{
		refreshTimer += delta;
		if (refreshTimer >= 1f)
		{
			refreshTimer -= 1f;
			Populate();
		}
	}

	private void UpdateName(string newName)
	{
		gameData.PlayerName = newName;
		if (connection.Mode == ConnectionMode.Host)
		{
			gameData.UpdatePlayer(1, newName, gameData.Team);
		}

		UpdateInfo();
		Populate();
	}

	private void UpdateTeam(int index)
	{
		gameData.Team = Data.TeamMappings[index];
		if (connection.Mode == ConnectionMode.Host)
		{
			gameData.UpdatePlayer(1, gameData.PlayerName, gameData.Team);
		}
		UpdateInfo();
		Populate();
	}

	public void UpdateInfo()
	{
		if (connection.Mode == ConnectionMode.Client)
		{
			connection.Update();
		}
		else
		{
			connection.Sync();
		}
	}

	private void VisibleChanged()
	{
		_nameBox.Text = gameData.PlayerName;
		_teamOption.Selected = hostJoinMenu.teamOption.Selected;
		Populate();
	}

	public void Disconnect()
	{

	}

	public void Populate()
	{
		List<long> keyList = new List<long>(gameData.PlayerList.Keys);
		keyList.Sort();
		for (int i = 0; i < _maxRows; i++)
		{
			if (i < gameData.PlayerList.Count)
			{
				PlayerData p = gameData.PlayerList[keyList[i]];

				_rowList[i].Populate(p.PlayerName, p.Team);
				_rowList[i].Show();
				if (connection.Mode == ConnectionMode.Host) GD.Print("Populate: " + p.PlayerName);
			}
			else
			{
				_rowList[i].Hide();
			}
		}
	}
}
