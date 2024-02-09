namespace TetraNet;

using Godot;
using System;
using static Data;

public partial class Spawner : Node
{
	public BlockType currentBlock;
	public BlockType[] nextBlocks;

	private BlockType[] _blockTypes;
	private Random _rand;

	public override void _Ready()
	{
		_rand = new Random();
		_blockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));
		currentBlock = PickRandomBlock();
		nextBlocks = new BlockType[3];
		for (int i = 0; i < 3; i++)
		{
			nextBlocks[i] = PickRandomBlock();
		}
	}

	private BlockType PickRandomBlock()
	{
		int randomIndex = _rand.Next(0, _blockTypes.Length);
		return _blockTypes[randomIndex];
	}

	public override void _Process(double delta)
	{
	}
}
