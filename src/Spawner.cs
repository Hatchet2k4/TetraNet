namespace TetraNet;

using Godot;
using System;
using static Data;
using System.Collections.Generic;

public partial class Spawner : Node
{
	public BlockType currentBlock;
	public List<BlockType> nextBlocks;
	private BlockType[] _blockTypes;
	private Random _rand;
	[Export] public PackedScene blockScene;

	public override void _Ready()
	{
		_rand = new Random();
		nextBlocks = new();

		_blockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));
		currentBlock = PickRandomBlock();

		for (int i = 0; i < 3; i++)
		{
			nextBlocks.Add(PickRandomBlock());
		}
	}

	public BlockType PickRandomBlock()
	{
		int randomIndex = _rand.Next(0, _blockTypes.Length);
		return _blockTypes[randomIndex];
	}

	private void SpawnNewBlock()
	{
		currentBlock = nextBlocks[0];
		nextBlocks.RemoveAt(0);
		nextBlocks.Add(PickRandomBlock());
	}

	public override void _Process(double delta)
	{


	}
}
