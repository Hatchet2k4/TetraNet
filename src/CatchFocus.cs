namespace TetraNet;

using Godot;
using System;

public partial class CatchFocus : ColorRect
{
	[Export] private MainField mainField;

	public void _on_gui_input(InputEvent e)
	{

		if (e is InputEventMouseButton)
		{
			GD.Print("Catchall");
			InputEventMouseButton ev = (InputEventMouseButton)e;

			mainField.GrabFocus();
			mainField.OnFocusEntered();
			mainField.processControls = true;
		}
	}

}
