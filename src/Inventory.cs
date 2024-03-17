namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using static Data;


public partial class Inventory : NinePatchRect
{
	[Export] private MainField _main;

	private PackedScene _itemScene = (PackedScene)ResourceLoader.Load("res://scenes/item.tscn");

	private List<Item> _items = new();

	public override void _Ready()
	{
		int index = 0;
		for (int y = 0; y < 3; y++)
		{
			for (int x = 0; x < 4; x++)
			{
				Item itm = _itemScene.Instantiate() as Item;
				AddChild(itm);
				_items.Add(itm);
				itm.Position = new Vector2(20 + x * 120, 24 + y * 72);
				itm.TooltipText = "Test Tooltip";
				itm.Texture = _main.itemTextures[index];
				index++;
			}
		}
	}

	public void AddItem(ItemType it)
	{
		Item i = _items[(int)it];
		if (i.Count < i.maxCount) i.Count++;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
