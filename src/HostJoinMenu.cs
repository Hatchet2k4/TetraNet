namespace TetraNet;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;

public partial class HostJoinMenu : Control
{
	[Export] private Title _title;
	[Export] public LineEdit nameBox;
	[Export] public OptionButton teamOption;
	[Export] private LineEdit _portBox;
	[Export] private CheckBox _observerBox;
	[Export] private LineEdit _addressBox;
	[Export] private Button _StartJoinServer;
	[Export] private ConnectionHandler _connection;
	[Export] private Lobby _lobby;
	[Export] private Label _status;
	[Export] private GameData _gameData;
	private string _publicIP = "";
	private ConnectionMode _mode;



	public override void _Ready()
	{
		SetProcess(false);
	}

	public async void SetMode(ConnectionMode mode)
	{
		nameBox.Text = ConfigData.PlayerName;
		_portBox.Text = ConfigData.Port.ToString();
		_status.Text = "";

		_mode = mode;


		if (mode == ConnectionMode.Host)
		{
			_addressBox.Editable = false;
			_StartJoinServer.Text = "Start Server";
			if (_publicIP == "")
			{
				_publicIP = await GetIPAddressAsync();
			}
			_addressBox.Text = _publicIP;
		}
		else
		{
			_addressBox.Editable = true;
			_addressBox.Text = ConfigData.JoinAddress;
			_StartJoinServer.Text = "Join Server";
		}
	}

	public async Task<string> GetIPAddressAsync()
	{
		string address = "";
		_addressBox.Text = "Finding IP...";
		try
		{
			using (System.Net.Http.HttpClient client = new())
			{
				HttpResponseMessage response = await client.GetAsync("http://checkip.dyndns.org/");
				response.EnsureSuccessStatusCode();
				using (Stream stream = await response.Content.ReadAsStreamAsync())
				using (StreamReader reader = new StreamReader(stream))
				{
					address = await reader.ReadToEndAsync();
				}
			}

			int first = address.IndexOf("Address: ") + 9;
			int last = address.LastIndexOf("</body>");
			address = address.Substring(first, last - first);
			GD.Print("Public IP found: " + address);

			return address;
		}
		catch (HttpRequestException ex)
		{
			GD.PrintErr($"HttpRequestException: {ex.Message}");
			return null;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Exception: {ex.Message}");
			return null;
		}
	}

	public void NameChanged(string newName)
	{
		ConfigData.PlayerName = newName;
		_gameData.PlayerName = newName;
		ValidateData();
	}

	public void TeamSelected(int index)
	{
		_gameData.Team = Data.TeamMappings[index];
	}
	public void ObserverChanged()
	{

	}
	public void PortChanged(string newPort)
	{
		int position = _portBox.CaretColumn;
		string stripped = Regex.Replace(newPort, "[^0-9]", "");
		int difference = newPort.Length - stripped.Length;
		_portBox.Text = stripped;
		_portBox.CaretColumn = position - difference;
		if (stripped.Length > 0)
		{
			ConfigData.Port = Int32.Parse(stripped);
		}
		ValidateData();
	}

	public void AddressChanged(string newAddress)
	{
		ConfigData.JoinAddress = newAddress;
		ValidateData();
	}

	public void ReturnToMenu()
	{
		ConfigData.Save();
		_title.ReturnFromSubMenu();
	}

	public void ValidateData()
	{
		_StartJoinServer.Disabled = !(nameBox.Text.Length > 0 && _portBox.Text.Length > 0 && _addressBox.Text.Length > 0);
	}

	public void JoinLobby()
	{
		ConfigData.Save();
		_connection.gameData.PlayerList.Clear();


		if (_mode == ConnectionMode.Host)
		{
			_connection.gameData.AddPlayer(1, _gameData.PlayerName, _gameData.Team);
			_status.Text = "Starting server...";
			_connection.StartServer(ConfigData.Port);
			_status.Text = "Server started.";
			Hide();
			_lobby.Show();
		}
		else
		{
			_status.Text = "Joining server...";
			_connection.ConnectToServer(ConfigData.JoinAddress, ConfigData.Port);
			_status.Text = "Connected.";
			Hide();
			_lobby.Show();
		}
	}
}
