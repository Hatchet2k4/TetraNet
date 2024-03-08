namespace TetraNet;

using Godot;
using System;

public partial class Lobby : Control
{
	[Export] private Title _title;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private ConnectionHandler connection;

	[Export] private LineEdit _nameBox;
	[Export] private OptionButton _teamOption;
	private string name;
	public string PlayerName
	{
		get
		{
			return name;
		}
		set
		{
			_nameBox.Text = value;
			name = value;
		}
	}

	public override void _Ready()
	{

		VisibilityChanged += VisibleChanged;
	}

	private void VisibleChanged()
	{
		//_nameBox.Text = hostJoinMenu.nameBox.Text; // ConfigData.PlayerName;
	}

	public void Disconnect()
	{

	}
}
