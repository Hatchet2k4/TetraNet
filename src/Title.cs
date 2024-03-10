namespace TetraNet;

using System.Collections.Generic;
using Godot;

public partial class Title : Node2D
{
	[Export] private AudioStreamPlayer _titleMusic;
	[Export] private Control _mainMenu;
	[Export] private HostJoinMenu _hostJoinMenu;
	[Export] private OptionsMenu _optionsMenu;
	[Export] private GameData _gameData;

	private List<Control> _menuList;

	private bool started = false;
	private double timer;

	public override void _Ready()
	{
		_menuList = new() {
			_mainMenu,
			_hostJoinMenu,
			_optionsMenu
		};

		ConfigData.Load();
		_titleMusic.VolumeDb = ConfigData.MusicVolume;
		_gameData.PlayerName = ConfigData.PlayerName;
	}

	public override void _Process(double delta)
	{
		timer += delta;
		if (!started && !_mainMenu.Visible && (timer > 2f || Gamepad.PressedStart() || Gamepad.PressedA() || Input.IsMouseButtonPressed(MouseButton.Left)))
		{
			_mainMenu.Show();
			started = true;
			SetProcess(false);
		}

	}

	public void ReturnFromSubMenu()
	{
		_hostJoinMenu.Hide();
		_optionsMenu.Hide();
		_mainMenu.Show();
	}

	public void SoloGame()
	{
		_titleMusic.Stop();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	public void StartServer()
	{
		_mainMenu.Hide();
		_hostJoinMenu.SetMode(ConnectionMode.Host);
		_hostJoinMenu.Show();
	}

	public void ConnectToServer()
	{
		_mainMenu.Hide();
		_hostJoinMenu.SetMode(ConnectionMode.Client);
		_hostJoinMenu.Show();
	}

	public void Options()
	{
		_mainMenu.Hide();
		_optionsMenu.Show();
	}

	public void SetupControls()
	{

	}

}
