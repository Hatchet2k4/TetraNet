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

	private static readonly string CFName = "settings.cfg";
	private static ConfigFile CF = new ConfigFile();

	public static void Save()
	{
		CF.SetValue("Settings", "PlayerName", PlayerName);
		CF.SetValue("Settings", "Port", Port);
		CF.SetValue("Settings", "JoinAddress", JoinAddress);
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
		}
		else
		{
			Save();
		}
	}

}


