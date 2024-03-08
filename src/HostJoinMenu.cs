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
	[Export] private LineEdit _nameBox;
	[Export] private OptionButton _teamOption;
	[Export] private LineEdit _portBox;
	[Export] private CheckBox _observerBox;
	[Export] private LineEdit _addressBox;
	[Export] private Button _StartJoinServer;

	private string _publicIP = "";
	private ConnectionMode _mode;

	public override void _Ready()
	{
		SetProcess(false);
	}

	public async void SetMode(ConnectionMode mode)
	{
		_nameBox.Text = ConfigData.PlayerName;
		_portBox.Text = ConfigData.Port.ToString();

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
		ValidateData();
	}

	public void TeamChanged()
	{

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
		_StartJoinServer.Disabled = !(_nameBox.Text.Length > 0 && _portBox.Text.Length > 0 && _addressBox.Text.Length > 0);
	}

	public void JoinLobby()
	{
		ConfigData.Save();
	}
}