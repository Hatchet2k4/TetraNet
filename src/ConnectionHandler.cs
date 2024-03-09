namespace TetraNet;

using Godot;
using System.Collections.Generic;

public enum ConnectionMode
{
	None,
	Host,
	Client
}

public partial class PlayerInfo : GodotObject
{
	public string Name;
	public string Team = "None";
}

public partial class ConnectionHandler : Node
{
	[Export] private Lobby _lobby;
	[Export] public GameData gameData;

	private ENetMultiplayerPeer _peer;
	public ConnectionMode Mode = ConnectionMode.None;


	public string PlayerName;
	public long Id;
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

	private void ConnectedToServer() //only runs on clients
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
		if (Mode == ConnectionMode.Host)
		{
			gameData.RemovePlayer(id);

			//Rpc("SyncPlayersToClients", AllPlayers);
		}
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
		Mode = ConnectionMode.Client;
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void SendPlayerInfo(string name, long id, string team) //send name data from client to server
	{
		if (!gameData.PlayerList.ContainsKey(id))
		{
			gameData.AddPlayer(id, name);
		}
	}

	/*
	[Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void SyncPlayersToClients(Godot.Collections.Dictionary<long, PlayerInfo> players)
	{
		if (Mode == ConnectionMode.Client)
		{
			AllPlayers = players;
		}
	}
	*/

}
