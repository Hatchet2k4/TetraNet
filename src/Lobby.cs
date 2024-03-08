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
			row.Position = new Vector2(0, row.Size.Y * i);
			_rowList.Add(row);
		}
	}

	private void VisibleChanged()
	{
		_nameBox.Text = hostJoinMenu.nameBox.Text; // ConfigData.PlayerName;
	}

	public void Disconnect()
	{

	}

	public void Populate()
	{
		for (int i = 0; i < _maxRows; i++)
		{
			if (i < connection.AllPlayers.Count)
			{
				PlayerInfo p = connection.AllPlayers[i];
				_rowList[i].Populate(p.Name, p.Team);
			}
			else _rowList[i].Hide();
		}
	}
}
