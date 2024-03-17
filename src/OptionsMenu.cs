namespace TetraNet;

using Godot;
using System;

public partial class OptionsMenu : Control
{
	[Export] private Title _title;
	[Export] public HSlider masterSlider;
	[Export] public HSlider musicSlider;
	[Export] public HSlider soundSlider;
	[Export] private AudioStreamPlayer testSound;
	[Export] private CheckBox fullscreenBox;
	private int _masterBus;
	private int _musicBus;
	private int _soundBus;
	private bool setup = false;

	public override void _Ready()
	{
		_masterBus = AudioServer.GetBusIndex("Master");
		_musicBus = AudioServer.GetBusIndex("Music");
		_soundBus = AudioServer.GetBusIndex("Sound");
		SetProcess(false);
	}


	public void ToggleFullScreen()
	{
		if (fullscreenBox.ButtonPressed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	public void SetVolumes()
	{
		masterSlider.Value = ConfigData.MasterVolume;
		musicSlider.Value = ConfigData.MusicVolume;
		soundSlider.Value = ConfigData.SoundVolume;
		AudioServer.SetBusVolumeDb(_masterBus, Mathf.LinearToDb(ConfigData.MasterVolume));
		AudioServer.SetBusVolumeDb(_musicBus, Mathf.LinearToDb(ConfigData.MusicVolume));
		AudioServer.SetBusVolumeDb(_soundBus, Mathf.LinearToDb(ConfigData.SoundVolume));
		setup = true;
	}

	public void MasterChanged(float value)
	{
		ConfigData.MasterVolume = value;
		AudioServer.SetBusVolumeDb(_masterBus, Mathf.LinearToDb(value));
		if (setup) testSound.Play();
	}

	public void MusicChanged(float value)
	{
		ConfigData.MusicVolume = value;
		AudioServer.SetBusVolumeDb(_musicBus, Mathf.LinearToDb(value));
	}

	public void SoundChanged(float value)
	{
		ConfigData.SoundVolume = value;
		AudioServer.SetBusVolumeDb(_soundBus, Mathf.LinearToDb(value));
		if (setup) testSound.Play();
	}

	public void ReturnToMenu()
	{
		ConfigData.Save();
		_title.ReturnFromSubMenu();
	}
}
