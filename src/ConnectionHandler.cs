namespace TetraNet;

using Godot;
using System.Collections.Generic;

public enum ConnectionMode
{
	None,
	Host,
	Join
}

public class PlayerInfo
{
	public string Name;
	public int Id;
	public string Team = "None";
	public bool Observer = false;
}

public partial class ConnectionHandler : Node
{
	[Export] private Lobby _lobby;
	private ENetMultiplayerPeer _peer;
	public ConnectionMode Mode = ConnectionMode.None;
	public List<PlayerInfo> AllPlayers = new();

	public string PlayerName;
	public int Id;
	public string Team;

	public bool Connected = false;

	public override void _Ready()
	{
		SetProcess(false);
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
	}

	private void ConnectionFailed()
	{
		GD.Print($"({Multiplayer.GetUniqueId()}) Connection failed.");
	}

	private void ConnectedToServer()
	{
		GD.Print($"({Multiplayer.GetUniqueId()}) Connected to server.");
		RpcId(1, "SendPlayerInfo", PlayerName, Id, Team);
	}

	private void PeerConnected(long id)
	{
		GD.Print($"({Multiplayer.GetUniqueId()}) Peer {id} connected.");
	}

	private void PeerDisconnected(long id)
	{
		GD.Print($"({Multiplayer.GetUniqueId()}) Peer {id} disconnected.");
	}

	public void StartServer(int port)
	{
		_peer = new ENetMultiplayerPeer();
		Mode = ConnectionMode.Host;
		var error = _peer.CreateServer(port, 24);
		if (error != Error.Ok)
		{
			GD.Print($"Hosting error: {error}.");
			return;
		}
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		Connected = true;
		GD.Print($"Server started with id {Multiplayer.GetUniqueId()}. Waiting for players.");
	}

	public void ConnectToServer(string address, int port)
	{
		Mode = ConnectionMode.Join;
		_peer = new ENetMultiplayerPeer();
		_peer.CreateClient(address, port);
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		Connected = true;
		GD.Print("Joining server.");
	}

	public void StartGame()
	{

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void SendPlayerInfo(string name, int id, string team)
	{
		PlayerInfo player = new()
		{
			Id = id,
			Name = name,
			Team = team
		};

		if (!AllPlayers.Contains(player))
		{
			AllPlayers.Add(player);
		}
		if (Mode == ConnectionMode.Host)
		{
			foreach (PlayerInfo p in AllPlayers)
			{
				Rpc("SendPlayerInfo", p.Name, p.Id, p.Team);
			}
		}
		_lobby.Populate();
	}

}
