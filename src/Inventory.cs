using Godot;
using System;
using TetraNet;

public partial class Inventory : NinePatchRect
{
	[Export] private MainField _main;

	private PackedScene _itemScene = (PackedScene)ResourceLoader.Load("res://scenes/item.tscn");

	public override void _Ready()
	{
		for (int x = 0; x < 4; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				Item itm = _itemScene.Instantiate() as Item;
				AddChild(itm);
				itm.Position = new Vector2(20 + x * 120, 24 + y * 72);
				itm.TooltipText = "Test Tooltip";
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}