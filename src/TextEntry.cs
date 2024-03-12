namespace TetraNet;

using Godot;
using System;


public partial class TextEntry : TextEdit
{
	[Export] private NinePatchRect _highlight;
	[Export] private MainField _mainField;
	[Export] private ChatLog _chatBox;
	[Export] private AudioStreamPlayer _chatSound;
	[Export] private GameData _gameData;
	[Export] private ConnectionHandler _connection;

	public void OnTextChanged()
	{
		int position = GetCaretColumn();
		string stripped = Text.Replace("\n", "").Replace("\r", "");
		int difference = Text.Length - stripped.Length;
		Text = stripped;
		SetCaretColumn(position - difference);
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (Text.Length > 0)
		{
			if (@event is InputEventKey keyEvent && keyEvent.Pressed && (keyEvent.Keycode == Key.Enter || keyEvent.Keycode == Key.KpEnter))
			{
				string text = _gameData.PlayerName + ": " + Text;
				_gameData.AddChat(_gameData.Id, text);
				Text = "";
				_chatBox.RefreshChat();
				_connection.AddChat(_gameData.Id, text);

			}
		}
	}

	public void OnFocusEntered()
	{
		_highlight.Visible = true;
		_highlight.Modulate = new Color(0.9f, 0.9f, 0f, 0.9f);
		Editable = true;
		if (_mainField is not null) _mainField.processControls = false;
	}

	public void OnFocusExited()
	{
		_highlight.Visible = false;
		Editable = false;
	}
}

