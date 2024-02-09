namespace TetraNet;

using Godot;
using System;
using static Data;

public partial class Spawner : Node
{
	public BlockType current_block;
	private BlockType[] _blockTypes;
	private Random rand;

	public override void _Ready()
	{
		rand = new Random();
		_blockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));
		PickRandomBlock();
	}

	private void PickRandomBlock()
	{
		int randomIndex = rand.Next(0, _blockTypes.Length);
		current_block = _blockTypes[randomIndex];
	}

	public override void _Process(double delta)
	{
	}
}
