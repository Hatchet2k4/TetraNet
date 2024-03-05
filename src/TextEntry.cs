using Godot;
using System;
using TetraNet;

public partial class TextEntry : TextEdit
{
	[Export] private NinePatchRect _highlight;
	[Export] private MainField _mainField;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnGuiInput(InputEvent @event)
	{
		/*
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			if (WithinBounds(mouseEvent.Position))
			{
				GD.Print("Clicked!");
				GrabFocus();
				_highlight.Visible = true;
				
			}
			else
			{
				GD.Print("Back to main");
				_highlight.Visible = false;
				_mainField.GrabFocus();
			}
		}
		*/
	}

	public bool WithinBounds(Vector2 pos)
	{
		return pos.X >= Position.X && pos.X < Position.X + Size.X && pos.Y >= Position.Y && pos.Y < Position.Y + Size.Y;
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
