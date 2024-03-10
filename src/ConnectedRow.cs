namespace TetraNet;

using Godot;

public partial class ConnectedRow : Control
{
	[Export] public RichTextLabel nameLabel;
	[Export] public RichTextLabel teamLabel;

	public override void _Ready()
	{
	}

	public void Populate(string name, string team)
	{

		switch (team)
		{
			case "Red":
				teamLabel.Text = $"[center][color=#DA0E23]{team}[/color][/center]";
				break;
			case "Blue":
				teamLabel.Text = $"[center][color=#009EFF]{team}[/color][/center]";
				break;
			case "Green":
				teamLabel.Text = $"[center][color=#3CE00F]{team}[/color][/center]";
				break;
			case "Yellow":
				teamLabel.Text = $"[center][color=#EDC200]{team}[/color][/center]";
				break;
			case "Orange":
				teamLabel.Text = $"[center][color=#F67F00]{team}[/color][/center]";
				break;
			case "Purple":
				teamLabel.Text = $"[center][color=#8121FF]{team}[/color][/center]";
				break;
			default:
				teamLabel.Text = $"[center]{team}[/center]";
				break;
		}

		nameLabel.Text = $"[center]{name}[/center]";
	}
}
