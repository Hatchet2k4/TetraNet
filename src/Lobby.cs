namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;

public partial class Lobby : Control
{
	[Export] private Title _title;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private ConnectionHandler connection;
	[Export] private LineEdit _nameBox;
	[Export] private OptionButton _teamOption;
	[Export] private PackedScene _connectedRow;
	[Export] private Control _rowListNode;
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

	private void VisibleChanged()
	{
		_nameBox.Text = hostJoinMenu.nameBox.Text;
	}

	public void Disconnect()
	{

	}

	public void Populate()
	{
		if (connection.Mode == ConnectionMode.Host) GD.Print("Connected: " + connection.gameData.PlayerList.Count.ToString());
		List<long> keyList = new List<long>(connection.gameData.PlayerList.Keys);
		keyList.Sort();
		for (int i = 0; i < _maxRows; i++)
		{
			if (i < connection.gameData.PlayerList.Count)
			{
				//PlayerInfo p = connection.gameData.AllPlayers[keyList[i]];
				string name = connection.gameData.PlayerList[keyList[i]];
				_rowList[i].Populate(name, "None");
				_rowList[i].Show();
				if (connection.Mode == ConnectionMode.Host) GD.Print(name);
			}
			else
			{
				_rowList[i].Hide();
			}
		}
	}
}
