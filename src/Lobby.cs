namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class Lobby : Control
{
	[Export] private Title _title;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private ConnectionHandler _connection;
	[Export] public LineEdit _nameBox;
	[Export] public OptionButton _teamOption;
	[Export] private PackedScene _connectedRow;
	[Export] private Control _rowListNode;
	[Export] private GameData _gameData;
	[Export] private Button _StartButton;

	private int _maxRows = 12;
	private List<ConnectedRow> _rowList;
	public double refreshTimer;

	public string PlayerName
	{
		get
		{
			return _connection.Name;
		}
		set
		{
			_nameBox.Text = value;
			_connection.Name = value;
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
		_gameData.PlayerName = newName;
		if (_connection.Mode == ConnectionMode.Host)
		{
			_gameData.UpdatePlayer(1, newName, _gameData.Team);
		}

		UpdateInfo();
		Populate();
	}

	private void UpdateTeam(int index)
	{
		_gameData.Team = Data.TeamMappings[index];
		if (_connection.Mode == ConnectionMode.Host)
		{
			_gameData.UpdatePlayer(1, _gameData.PlayerName, _gameData.Team);
		}
		UpdateInfo();
		Populate();
	}

	public void UpdateInfo()
	{
		if (_connection.Mode == ConnectionMode.Client)
		{
			_connection.Update();
		}
		else
		{
			_connection.Sync();
		}
	}

	private void VisibleChanged()
	{
		_nameBox.Text = _gameData.PlayerName;
		_teamOption.Selected = hostJoinMenu.teamOption.Selected;
		_StartButton.Visible = !(_connection.Mode == ConnectionMode.Client);

		Populate();
	}

	public void Disconnect()
	{
		_connection.Disconnect();
		hostJoinMenu.ReturnFromLobby();
	}

	public void Populate()
	{
		List<long> keyList = new List<long>(_gameData.PlayerList.Keys);
		keyList.Sort();
		for (int i = 0; i < _maxRows; i++)
		{
			if (i < _gameData.PlayerList.Count)
			{
				PlayerData p = _gameData.PlayerList[keyList[i]];

				_rowList[i].Populate(p.PlayerName, p.Team);
				_rowList[i].Show();
			}
			else
			{
				_rowList[i].Hide();
			}
		}
	}

	public void StartGame()
	{
		
	}
}
