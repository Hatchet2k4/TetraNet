namespace TetraNet;

using Godot;

public partial class ConnectionHandler : Control
{
	[Export] private int _port = 13770;
	[Export] private string _address = "127.0.0.1";

	private ENetMultiplayerPeer _peer;
	public override void _Ready()
	{

		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
	}

	private void ConnectionFailed()
	{
		GD.Print("Connection failed.");
	}


	private void ConnectedToServer()
	{
		GD.Print("Connected to server.");
	}


	private void PeerConnected(long id)
	{
		GD.Print($"Peer {id} connected.");
	}


	private void PeerDisconnected(long id)
	{
		GD.Print($"Peer {id} disconnected.");
	}

	private void StartServer()
	{
		_peer = new ENetMultiplayerPeer();
		var error = _peer.CreateServer(_port, 24);
		if (error != Error.Ok)
		{
			GD.Print($"Hosting error: {error}.");
			return;
		}
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Waiting for players.");
	}

	private void ConnectToServer()
	{
		_peer = new ENetMultiplayerPeer();
		_peer.CreateClient(_address, _port);
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Joining server.");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
