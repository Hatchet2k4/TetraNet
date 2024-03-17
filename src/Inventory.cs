namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using static Data;


public partial class Inventory : NinePatchRect
{
	[Export] private MainField _main;

	[Export] private TextureRect _target;
	private PackedScene _itemScene = (PackedScene)ResourceLoader.Load("res://scenes/item.tscn");
	private List<Item> _items = new();

	public int targetIndex;

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
				itm.inventory = this;
				itm.index = index;
				itm.Position = new Vector2(20 + x * 120, 24 + y * 72);
				itm.TooltipText = "Test Tooltip";
				itm.Texture = _main.itemTextures[index];
				index++;
			}
		}
		Target(0);
	}

	public void Target(int index)
	{
		_target.Position = _items[index].Position - new Vector2(2, 2);
		targetIndex = index;
	}

	public void AddItem(ItemType it)
	{
		Item item = _items[(int)it];
		if (item.Count < item.maxCount) item.Count++;
	}

	public void UseItem()
	{
		ItemType it = (ItemType)targetIndex;
		Item item = _items[(int)it];
		if (item.Count > 0) item.Count--;
	}


}
