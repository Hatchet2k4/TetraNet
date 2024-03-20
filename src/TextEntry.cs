namespace TetraNet;

using Godot;
using System;


public partial class TextEntry : TextEdit
{
	[Export] private NinePatchRect _highlight;
	[Export] private MainField _mainField;
	[Export] private ChatLog _chatBox;
	[Export] private GameData _gameData;
	[Export] private ConnectionHandler _connection;

	private bool processControls;


	public void OnTextChanged()
	{
		int position = GetCaretColumn();
		string stripped = Text.Replace("\n", "").Replace("\r", "");
		int difference = Text.Length - stripped.Length;
		Text = stripped;
		SetCaretColumn(position - difference);
	}

	public override void _Process(double delta)
	{
		if (HasFocus())
		{
			processControls = true; //doing this to delay input a frame so it doesn't consume the tab press	from main
		}
		else
		{
			processControls = false;
		}
	}

	public void OnGuiInput(InputEvent @event)
	{
		if (_connection == null) //haaack
		{
			_connection = _mainField._root.connection;
			_gameData = _mainField._root.gameData;
		}

		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Tab)
			{
				GetViewport().SetInputAsHandled();
				if (processControls)
				{
					GD.Print("chattab");
					OnFocusExited();
				}
			}
			if (Text.Length > 0)
			{
				if (keyEvent.Keycode == Key.Enter || keyEvent.Keycode == Key.KpEnter)
				{
					string text = _gameData.PlayerName + ": " + Text;
					_gameData.AddChat(_gameData.Id, text);
					Text = "";
					_chatBox.RefreshChat(1);
					if (_connection.Mode != ConnectionMode.None) _connection.AddChat(_gameData.Id, text);
				}
			}
		}
	}

	public void OnFocusEntered()
	{
		_highlight.Visible = true;
		_highlight.Modulate = new Color(0f, 0.9f, 0f, 1f);
		Editable = true;

		if (_mainField is not null)
		{
			_mainField.OnFocusExited();
		}

	}

	public void OnFocusExited()
	{
		_highlight.Visible = false;
		Editable = false;
		if (_mainField is not null)
		{
			_mainField.GrabFocus();
			_mainField.OnFocusEntered();
		}
	}
}

