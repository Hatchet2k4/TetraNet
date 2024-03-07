namespace TetraNet;

using Godot;

public partial class HostMenu : Control
{

	[Export] public OptionButton teamOption;
	[Export] public LineEdit portBox;

	private int port;

	public override void _Ready()
	{

		SetProcess(false);
	}

}
