namespace TetraNet;

using Godot;
using static Data;

public partial class MainField : Control
{
	[Export] private PackedScene _block;
	[Export] private Spawner _spawner;

	public override void _Ready()
	{

	}

	public void SpawnBlock(BlockType t)
	{

		//var b = _block.Instantiate();
		//b.
	}

	public override void _Process(double delta)
	{
	}
}
