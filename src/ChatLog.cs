namespace TetraNet;

using Godot;
using System;
using System.Text;


public partial class ChatLog : RichTextLabel
{
	[Export] private GameData _gameData;
	private int chatCount;
	[Export] private MainField main;

	[Export] private AudioStreamPlayer _chatSound;

	[Export] private AudioStreamPlayer _chatSound2;
	public override void _Ready()
	{
	}


	public override void _Process(double delta)
	{
		if (_gameData == null) _gameData = main._root.gameData;

		if (chatCount < _gameData.ChatLog.Count)
		{
			RefreshChat();
			chatCount = _gameData.ChatLog.Count;
		}
	}

	public void RefreshChat(int chatSound = 0)
	{
		if (chatSound > 0) _chatSound2.Play();
		else _chatSound.Play();
		//if (ScrollBar.)

		StringBuilder fullText = new();
		for (int i = 0; i < _gameData.ChatLog.Count; i++)
		{
			ChatEntry line = _gameData.ChatLog[i];
			if (line.Id == _gameData.Id)
			{
				fullText.AppendLine("[color=#F9FF8F]" + line.Text + "[/color]");
			}
			else fullText.AppendLine(line.Text);
		}
		Text = fullText.ToString();
	}
}
