namespace TetraNet;

using Godot;

public partial class HostMenu : Control
{

	[Export] public OptionButton teamOption;


	public override void _Ready()
	{

		SetProcess(false);
	}

}
