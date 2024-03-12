namespace TetraNet;

using Godot;
using System.Collections.Generic;
using System.Text.Json;
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
		RpcId(1, "SendPlayerInfo", gameData.PlayerName, Multiplayer.GetUniqueId(), gameData.Team);
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
			string json = JsonSerializer.Serialize(gameData.PlayerList);
			Rpc("SyncPlayersToClients", json);
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
		GD.Print($"Server started. Waiting for players.");
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

	public void Disconnect()
	{
		_peer.Close();
	}

	public void StartGame()
	{

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void Update()
	{
		RpcId(1, "UpdatePlayerInfo", gameData.PlayerName, Multiplayer.GetUniqueId(), gameData.Team);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void Sync()
	{
		string json = JsonSerializer.Serialize(gameData.PlayerList);
		Rpc("SyncPlayersToClients", json);
	}

	public void AddChat(long id, string text)
	{
		Rpc("RPCAddChat", id, text);
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void SendPlayerInfo(string name, long id, string team) //send name data from client to server
	{
		if (!gameData.PlayerList.ContainsKey(id))
		{
			gameData.AddPlayer(id, name, team);
		}

		if (Mode == ConnectionMode.Host)
		{
			string json = JsonSerializer.Serialize(gameData.PlayerList);
			Rpc("SyncPlayersToClients", json);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void RPCAddChat(long id, string text) //send name data from client to server
	{
		gameData.AddChat(id, text);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void UpdatePlayerInfo(string name, long id, string team) //send name data from client to server
	{
		gameData.UpdatePlayer(id, name, team);
		if (Mode == ConnectionMode.Host)
		{
			Sync();
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void SyncPlayersToClients(string json)
	{
		if (Mode == ConnectionMode.Client)
		{
			Dictionary<long, PlayerData> list = LoadJsonFromString<Dictionary<long, PlayerData>>(json);
			GD.Print("SyncPlayersToClients: " + json);
			gameData.PlayerList = list;
		}
	}


	public static T LoadJsonFromString<T>(string str)
	{
		try
		{
			return JsonSerializer.Deserialize<T>(str);
		}
		catch
		{
			return default;
		}
	}

}
