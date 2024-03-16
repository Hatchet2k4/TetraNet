namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


public partial class ConfigData : Node
{
	public static string PlayerName { get; set; }
	public static int Port { get; set; }
	public static string JoinAddress { get; set; }
	public static float MasterVolume { get; set; }
	public static float MusicVolume { get; set; }
	public static float SoundVolume { get; set; }


	private static readonly string CFName = "settings.cfg";
	private static ConfigFile CF = new ConfigFile();

	public static void Save()
	{
		CF.SetValue("Settings", "PlayerName", PlayerName);
		CF.SetValue("Settings", "Port", Port);
		CF.SetValue("Settings", "JoinAddress", JoinAddress);
		CF.SetValue("Settings", "MasterVolume", MasterVolume);
		CF.SetValue("Settings", "MusicVolume", MusicVolume);
		CF.SetValue("Settings", "SoundVolume", SoundVolume);
		GD.Print($"Saving settings: {OS.GetUserDataDir()}/{CFName}");
		CF.Save($"user://{CFName}");
	}

	public static void Load()
	{
		if (CF.Load($"user://{CFName}") == Error.Ok)
		{
			GD.Print($"Loaded settings: {OS.GetUserDataDir()}/{CFName}");
			PlayerName = (string)CF.GetValue("Settings", "PlayerName", "");
			Port = (int)CF.GetValue("Settings", "Port", 13370);
			JoinAddress = (string)CF.GetValue("Settings", "JoinAddress", "");
			MasterVolume = (float)CF.GetValue("Settings", "MasterVolume", Mathf.LinearToDb(0.8f));
			MusicVolume = (float)CF.GetValue("Settings", "MusicVolume", Mathf.LinearToDb(0.8f));
			SoundVolume = (float)CF.GetValue("Settings", "SoundVolume", Mathf.LinearToDb(0.8f));
			if (Port == 0) Port = 13370;
		}
		else
		{
			Save();
		}
	}

}


