namespace TetraNet;

using Godot;
using System;
using System.Text;


public partial class ChatLog : RichTextLabel
{
	[Export] private GameData _gameData;

	public override void _Ready()
	{
		SetProcess(false);
	}


	public override void _Process(double delta)
	{
	}

	public void RefreshChat()
	{
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
		GD.Print(Text);
	}
}
