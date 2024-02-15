namespace TetraNet;

using Godot;
using static Data;

public partial class MainField : Control
{

	[Export] private Spawner _spawner;
	[Export] private TextureRect _grid;

	[Export] private TextureRect _nextGrid;

	private Piece[,] _gridData;

	private Signal gameover;
	private Block _currentBlock;



	public override void _Ready()
	{
		_gridData = new Piece[GRID_W, GRID_H];
		SpawnNewBlock(_spawner.currentBlock);
	}

	public void SpawnNewBlock(BlockType t)
	{
		_currentBlock = _spawner.blockScene.Instantiate() as Block;
		_currentBlock.Initialize(Data.blockResources[t]);
		_currentBlock.Position = _grid.Position + new Vector2(24 * 10, 0);
		AddChild(_currentBlock);
	}

	public override void _Process(double delta)
	{
	}
}
