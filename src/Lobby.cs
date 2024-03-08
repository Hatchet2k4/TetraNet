namespace TetraNet;

using Godot;
using System;

public partial class Lobby : Control
{
	[Export] private Title _title;
	[Export] private HostJoinMenu hostJoinMenu;
	[Export] private ConnectionHandler connection;


	public override void _Ready()
	{
	}

	public void Disconnect()
	{

	}
}
