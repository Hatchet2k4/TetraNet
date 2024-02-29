namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;

public partial class MainField : Control
{
	[Export] private Spawner _spawner;
	[Export] private TextureRect _grid;
	[Export] private TextureRect _nextGrid;

	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");

	private double _fallTime;

	private Piece[,] _gridData;

	private Signal gameover;
	private Block _currentBlock;
	private double _time;
	private double _holdTime;
	private double _holdRate = 0.1f;

	public Vector2 blockPosition;

	public override void _Ready()
	{
		_fallTime = 0.75f;
		_time = 0;
		_gridData = new Piece[GRID_W, GRID_H];
		SpawnNewBlock(_spawner.currentBlock);
	}

	public void SpawnNewBlock(BlockType t)
	{
		GD.Print("SpawnNewBlock");
		_currentBlock = _spawner.blockScene.Instantiate() as Block;
		_currentBlock.Initialize(blockResources[t]);

		blockPosition = new Vector2(4, 0) + gridSpawnPositions[t];
		_currentBlock.Position = _grid.Position + (blockPosition * GRID_SIZE);

		AddChild(_currentBlock);
	}

	public override void _Process(double delta)
	{
		_time += delta;
		if (_time > _fallTime)
		{
			_time -= _fallTime;
			Move(DOWN);
		}
		else
		{
			ProcessInput();
		}
	}

	public void ProcessInput()
	{
		if (Gamepad.UpPressed())
		{
			Rotate(LEFT);
		}
		if (Gamepad.LeftPressed())
		{
			Move(LEFT);
		}
		else if (Gamepad.RightPressed())
		{
			Move(RIGHT);
		}
		else if (Gamepad.DownPressed())
		{
			Move(DOWN);
		}
	}

	public void Rotate(Vector2 direction)
	{
		List<Vector2> rotationMatrix = rotations[direction];
		List<Vector2> tetromino_cells = Cells[_currentBlock.BlockType];

		for (int i = 0; i < 4; i++)
		{
			Vector2 cell = tetromino_cells[i];
			var coordinates = rotationMatrix[0] * cell.X + rotationMatrix[1] * cell.Y;
			_currentBlock.cells[i] = coordinates;
		}
		_currentBlock.ResetPositions();
	}

	public void Move(Vector2 direction)
	{
		if (CheckCollisions(direction) == false)
		{
			blockPosition += direction;
			_currentBlock.Position = _grid.Position + (blockPosition * GRID_SIZE);

			for (int i = 0; i < 4; i++)
			{
				Vector2 testPosition = blockPosition + _currentBlock.cells[i];
				int x = (int)testPosition.X;
				int y = (int)testPosition.Y;
				GD.Print(testPosition);
			}
			if (direction == DOWN)
			{
				_time = 0f;
			}

		}
		else
		{
			if (direction == DOWN)
			{
				Land();
			}
		}
	}

	public bool CheckGrid(int x, int y)
	{
		if (x < 0 || x >= GRID_W || y < 0 || y >= GRID_H)
		{
			//GD.Print("BoundsCollision");
			return true;
		}
		if (_gridData[x, y] != null) return true;
		return false;
	}

	public bool CheckCollisions(Vector2 direction)
	{
		for (int i = 0; i < 4; i++)
		{
			Vector2 testPosition = blockPosition + direction + _currentBlock.cells[i];
			int x = (int)testPosition.X;
			int y = (int)testPosition.Y;

			if (CheckGrid(x, y))
			{
				return true;
			}
		}
		return false;
	}

	public void Land()
	{
		GD.Print("Land");
		for (int i = 0; i < 4; i++)
		{
			Vector2 position = blockPosition + _currentBlock.cells[i];
			int x = (int)position.X;
			int y = (int)position.Y;

			Piece p = _currentBlock.GetPiece(i);
			_currentBlock.RemoveChild(p);
			p.Position = _grid.Position + position * GRID_SIZE;
			AddChild(p);
			_gridData[x, y] = p;
		}
		RemoveChild(_currentBlock);
		SpawnNewBlock(_spawner.PickRandomBlock());
	}
}
