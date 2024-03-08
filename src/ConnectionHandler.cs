namespace TetraNet;

using Godot;

public partial class ConnectionHandler : Control
{
	private ENetMultiplayerPeer _peer;
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

	public void StartServer(int port)
	{
		_peer = new ENetMultiplayerPeer();
		var error = _peer.CreateServer(port, 24);
		if (error != Error.Ok)
		{
			GD.Print($"Hosting error: {error}.");
			return;
		}
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Waiting for players.");
	}

	public void ConnectToServer(string address, int port)
	{
		_peer = new ENetMultiplayerPeer();
		_peer.CreateClient(address, port);
		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
		GD.Print("Joining server.");
	}
}
