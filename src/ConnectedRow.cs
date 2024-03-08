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
		nameLabel.Text = $"[center]{name}[/center]";
		teamLabel.Text = $"[center]{team}[/center]";
	}
}
