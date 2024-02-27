namespace TetraNet;

using Godot;

public static class Gamepad
{
	private const string CONSOLE = "console";
	private const string QUICKSAVE = "quicksave";
	private const string QUICKLOAD = "quickload";
	private const string SCREENSHOT = "screenshot";
	private const string FULLSCREEN = "fullscreen";

	private const string BUTTON_A = "button_a";
	private const string BUTTON_B = "button_b";
	private const string BUTTON_X = "button_x";
	private const string BUTTON_Y = "button_y";
	private const string BUTTON_L = "button_l";
	private const string BUTTON_R = "button_r";
	private const string BUTTON_START = "button_start";
	private const string BUTTON_SELECT = "button_select";

	private const string DPAD_UP = "dpad_up";
	private const string DPAD_DOWN = "dpad_down";
	private const string DPAD_LEFT = "dpad_left";
	private const string DPAD_RIGHT = "dpad_right";

	public static bool FullscreenTogglePressed()
	{
		return Input.IsActionJustPressed(FULLSCREEN);
	}

	public static bool ConsolePressed()
	{
		return Input.IsActionJustPressed(CONSOLE);
	}

	public static bool ScreenshotPressed()
	{
		return Input.IsActionJustPressed(SCREENSHOT);
	}

	public static bool QuicksavePressed()
	{
		return Input.IsActionJustPressed(QUICKSAVE);
	}

	public static bool QuickloadPressed()
	{
		return Input.IsActionJustPressed(QUICKLOAD);
	}

	public static bool PressedA()
	{
		return Input.IsActionJustPressed(BUTTON_A);
	}

	public static bool PressedB()
	{
		return Input.IsActionJustPressed(BUTTON_B);
	}

	public static bool PressedL()
	{
		return Input.IsActionJustPressed(BUTTON_L);
	}

	public static bool PressedR()
	{
		return Input.IsActionJustPressed(BUTTON_R);
	}

	public static bool PressedX()
	{
		return Input.IsActionJustPressed(BUTTON_X);
	}

	public static bool PressedY()
	{
		return Input.IsActionJustPressed(BUTTON_Y);
	}

	public static bool PressedStart()
	{
		return Input.IsActionJustPressed(BUTTON_START);
	}

	public static bool PressedSelect()
	{
		return Input.IsActionJustPressed(BUTTON_SELECT);
	}

	public static bool HeldA()
	{
		return Input.IsActionPressed(BUTTON_A);
	}

	public static bool HeldB()
	{
		return Input.IsActionPressed(BUTTON_B);
	}

	public static bool HeldX()
	{
		return Input.IsActionPressed(BUTTON_X);
	}

	public static bool HeldY()
	{
		return Input.IsActionPressed(BUTTON_Y);
	}

	public static bool HeldL()
	{
		return Input.IsActionPressed(BUTTON_L);
	}

	public static bool HeldR()
	{
		return Input.IsActionPressed(BUTTON_R);
	}

	public static bool UpPressed()
	{
		return Input.IsActionJustPressed(DPAD_UP);
	}

	public static bool DownPressed()
	{
		return Input.IsActionJustPressed(DPAD_DOWN);
	}

	public static bool LeftPressed()
	{
		return Input.IsActionJustPressed(DPAD_LEFT);
	}

	public static bool RightPressed()
	{
		return Input.IsActionJustPressed(DPAD_RIGHT);
	}

	public static bool UpHeld()
	{
		return Input.IsActionPressed(DPAD_UP);
	}

	public static bool DownHeld()
	{
		return Input.IsActionPressed(DPAD_DOWN);
	}

	public static bool LeftHeld()
	{
		return Input.IsActionPressed(DPAD_LEFT);
	}

	public static bool RightHeld()
	{
		return Input.IsActionPressed(DPAD_RIGHT);
	}

	public static bool DpadNextPressed()
	{
		return DownPressed() || RightPressed();
	}

	public static bool DpadPreviousPressed()
	{
		return LeftPressed() || UpPressed();
	}

	public static Vector2 GetDpadAxis()
	{
		var input = new Vector2();
		if (UpHeld())
		{
			input.Y = -1f;
		}
		else if (DownHeld())
		{
			input.Y = 1f;
		}
		if (LeftHeld())
		{
			input.X = -1f;
		}
		else if (RightHeld())
		{
			input.X = 1f;
		}
		return input;
	}
}
