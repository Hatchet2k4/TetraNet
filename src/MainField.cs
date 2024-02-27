namespace TetraNet;

using Godot;
using static Data;

public partial class MainField : Control
{
	[Export] private Spawner _spawner;
	[Export] private TextureRect _grid;
	[Export] private TextureRect _nextGrid;

	private double _fallTime;

	private Piece[,] _gridData;

	private Signal gameover;
	private Block _currentBlock;

	private double _time;
	private double _holdTime;
	private double _holdRate = 0.1f;



	public override void _Ready()
	{
		_fallTime = 0.75f;
		_time = 0;
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
		_time += delta;
		if (_time > _fallTime)
		{
			_time -= _fallTime;
			MoveDown();
		}
		else
		{
			ProcessInput();
		}
	}

	public void ProcessInput()
	{
		if (Gamepad.LeftPressed())
		{
			MoveLeft();
		}
		else if (Gamepad.RightPressed())
		{
			MoveRight();
		}

	}

	public void MoveLeft()
	{
		_currentBlock.Position += LEFT * GRID_SIZE;
	}

	public void MoveRight()
	{
		_currentBlock.Position += RIGHT * GRID_SIZE;
	}

	public void MoveDown()
	{
		_currentBlock.Position += DOWN * GRID_SIZE;
	}

}
