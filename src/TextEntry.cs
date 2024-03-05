using Godot;
using System;
using TetraNet;

public partial class TextEntry : TextEdit
{
	[Export] private NinePatchRect _highlight;
	[Export] private MainField _mainField;
	[Export] private TextEdit _chatBox;
	[Export] private AudioStreamPlayer _chatSound;

	public void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Keycode == Key.Enter)
		{
			_chatBox.Text += Text;
			Text = "";
			_chatSound.Play();
		}
	}

	public void OnFocusEntered()
	{
		GD.Print("Focused!");
		_highlight.Visible = true;
		_highlight.Modulate = new Color(0.9f, 0.9f, 0f, 0.9f);
		Editable = true;
		_mainField.processControls = false;
	}

	public void OnFocusExited()
	{
		GD.Print("UnFocused!");
		_highlight.Visible = false;
		Editable = false;
	}
}

