namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;
using System;

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

	private double fadeTime = 0.5;

	private double repeatRate = 0.1f;
	private bool _downHeld;

	public Vector2 blockPosition;

	private List<int> _lines = new List<int>(); //stores list of rows for completed lines

	private int clearIndex; //column to clear for row removal effect
	private double clearTimer;

	public override void _Ready()
	{
		_fallTime = 0.75f;
		_time = 0;
		_gridData = new Piece[GRID_W, GRID_H];
		_lines = new();
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
		if (_lines.Count > 0) //processing lines
		{
			clearTimer += delta;
			if (clearTimer >= 0.02f)
			{
				clearTimer -= 0.02f;
				for (int i = 0; i < _lines.Count; i++)
				{
					int y = _lines[i];
					RemoveChild(_gridData[clearIndex, y]);
					_gridData[clearIndex, y] = null;
				}
				clearIndex++;
				if (clearIndex == GRID_W)
				{
					RemoveBlankLines();
					ResetPiecePositions();
					_lines.Clear();
					clearTimer = 0f;
					clearIndex = 0;
					SpawnNewBlock(_spawner.PickRandomBlock());
				}
			}
			return; //may refactor this later to allow inventory input even while lines are being cleared
		}

		_time += delta;
		if (_time > _fallTime)
		{
			_time -= _fallTime;
			Move(DOWN);
		}
		else
		{
			ProcessInput(delta);
		}
	}

	public void ProcessInput(double delta)
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
		else if (Gamepad.DownHeld() || Gamepad.RightHeld() || Gamepad.LeftHeld())
		{
			if (!_downHeld && _holdTime == 0d)
			{
				_holdTime = _holdRate;
				_downHeld = true;
			}
			else if (_holdTime <= 0d)
			{
				_holdTime += _holdRate;
				if (Gamepad.DownHeld()) Move(DOWN);
				else if (Gamepad.LeftHeld()) Move(LEFT);
				else if (Gamepad.RightHeld()) Move(RIGHT);
			}
			else
			{
				_holdTime -= delta;
			}
			return;
		}
		_downHeld = false;
		_holdTime = 0d;
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
		CheckLines();
		if (_lines.Count == 0) SpawnNewBlock(_spawner.PickRandomBlock());
	}

	public void RemoveBlankLines()
	{
		foreach (int y in _lines)
		{
			for (int x = 0; x < GRID_W; x++)
			{
				for (int dy = y; dy >= 0; dy--)
				{
					if (dy > 0) _gridData[x, dy] = _gridData[x, dy - 1];
					else _gridData[x, dy] = null;
				}
			}
		}
	}

	public void ResetPiecePositions()
	{
		for (int y = 0; y < GRID_H; y++)
		{
			for (int x = 0; x < GRID_W; x++)
			{
				if (_gridData[x, y] != null)
				{
					_gridData[x, y].Position = _grid.Position + new Vector2(x * GRID_SIZE, y * GRID_SIZE);
				}
			}
		}
	}

	public void CheckLines()
	{
		int numlines = 0;
		for (int y = 0; y < GRID_H; y++)
		{

			for (int x = 0; x < GRID_W; x++)
			{
				if (_gridData[x, y] == null)
				{
					break;
				}

				if (x + 1 == GRID_W) //made it all the way across, found a line!
				{
					numlines += 1;
					_lines.Add(y); //found on line y
				}
			}
		}

	}

}
