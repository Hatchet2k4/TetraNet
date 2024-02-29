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

		BlockType[] types = {
			BlockType.I,
			BlockType.J,
			BlockType.L,
			BlockType.O,
			BlockType.S,
			BlockType.T,
			BlockType.Z
		};

		for (int i = 0; i < 3; i++)
		{
			Block b = _spawner.blockScene.Instantiate() as Block;
			BlockType bt = _spawner.nextBlocks[i];
			b.Initialize(blockResources[bt]);
			//b.Initialize(blockResources[types[i]]);
			b.Position = _nextGrid.Position + new Vector2(GRID_SIZE, GRID_SIZE * (3 * i)) + (nextSpawnPositions[bt] * GRID_SIZE);
			AddChild(b);
			nextBlocks.Add(b);
		}
	}
}
