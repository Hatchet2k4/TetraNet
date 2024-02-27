namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;

public partial class NextGrid : Control
{
	[Export] private Spawner _spawner;
	[Export] private TextureRect _nextGrid;
	public List<Block> nextBlocks;

	public override void _Ready()
	{
		nextBlocks = new();
		Populate();
		SetProcess(false);
	}

	public void Populate()
	{
		nextBlocks.Clear();
		for (int i = 0; i < 3; i++)
		{
			Block b = _spawner.blockScene.Instantiate() as Block;
			b.Initialize(Data.blockResources[_spawner.nextBlocks[i]]);
			b.Position = _nextGrid.Position + new Vector2(48, 48 * (4 * i));
			AddChild(b);
			nextBlocks.Add(b);
		}
	}
}
