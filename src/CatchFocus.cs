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
			InputEventMouseButton ev = (InputEventMouseButton)e;
			GD.Print("Catchall");
			mainField.GrabFocus();
			mainField.processControls = true;
		}
	}

}
