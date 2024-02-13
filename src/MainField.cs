namespace TetraNet;

using Godot;
using static Data;

public partial class MainField : Control
{
	[Export] private PackedScene _blockScene;
	[Export] private Spawner _spawner;
	[Export] private TextureRect _grid;

	private Signal gameover;
	private Block _currentBlock;

	public override void _Ready()
	{
		SpawnNewBlock(BlockType.T);
	}

	public void SpawnNewBlock(BlockType t)
	{
		_currentBlock = _blockScene.Instantiate() as Block;
		_currentBlock.Initialize(Data.blockResources[t]);	
		_currentBlock.Position = _grid.Position + new Vector2(24 * 10, 0);
		AddChild(_currentBlock);		
	}

	public override void _Process(double delta)
	{
	}
}
