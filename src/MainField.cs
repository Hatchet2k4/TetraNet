namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;


public partial class MainField : Control
{
	[Export] private Main _root;
	[Export] private Spawner _spawner;
	[Export] private TextureRect _grid;
	[Export] private NextGrid _nextGrid;
	[Export] private HoldGrid _holdGrid;
	[Export] private Label _lblLines;

	[Export] private Countdown _countdown;
	[Export] private AudioStreamPlayer music;
	[Export] private AudioStreamPlayer landSound;
	[Export] private AudioStreamPlayer clearSound;
	[Export] private AudioStreamPlayer clear2Sound;
	[Export] private AudioStreamPlayer clear3Sound;
	[Export] private AudioStreamPlayer holdSound;
	[Export] private AudioStreamPlayer dropSound;
	[Export] private AudioStreamPlayer moveSound;
	[Export] private AudioStreamPlayer rotateSound;

	public List<MiniField> miniFields;
	private Texture2D _ghostTexture;

	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");

	private PackedScene _minifieldScene = (PackedScene)ResourceLoader.Load("res://scenes/mini_field.tscn");

	private PackedScene _gradientScene = (PackedScene)ResourceLoader.Load("res://scenes/gradient.tscn");

	private double _fallTime;

	private Piece[,] _gridData;

	private Signal gameover;
	private Block _currentBlock;
	private Block _ghostBlock;

	private double _time;
	private double _holdTime;
	private double _holdRate = 0.1f;

	private double fadeTime = 0.5;

	private bool _downHeld;

	public Vector2 blockPosition;
	public Vector2 ghostPosition;

	private List<int> _lines = new List<int>(); //stores list of rows for completed lines

	private int clearIndex; //column to clear for row removal effect
	private double clearTimer;

	public bool swapped;

	public int totalLines;

	public bool processControls = true;



	public override void _Ready()
	{

		_ghostTexture = GD.Load("res://gfx/Ghost.png") as Texture2D;
		_fallTime = 0.5f;
		_time = 0;
		_gridData = new Piece[GRID_W, GRID_H];
		_lines = new();
		miniFields = new();
		CreateMiniFields();
		SetProcess(false);

	}

	public void CreateMiniFields()
	{
		float ypos = 30;
		for (int x = 0; x < 6; x++)
		{
			MiniField mf = _minifieldScene.Instantiate() as MiniField;
			_root.CallDeferred("add_child", mf);
			float xpos = 888 + (256 + 16) * x;
			mf.Position = new Vector2(xpos, ypos);
			miniFields.Add(mf);
		}
		ypos = 574;
		for (int x = 1; x < 6; x++)
		{
			MiniField mf = _minifieldScene.Instantiate() as MiniField;
			_root.CallDeferred("add_child", mf);
			float xpos = 888 + (256 + 16) * x;
			mf.Position = new Vector2(xpos, ypos);
			miniFields.Add(mf);
		}

	}

	public void NewGame()
	{
		swapped = false;
		totalLines = 0;
		SpawnNewBlock(_spawner.GetNextBlock());
		_countdown.Start();
		music.Play();
		SetProcess(true);
	}

	public void StopGame()
	{

	}

	public void SpawnNewBlock(BlockType t)
	{
		_currentBlock = _spawner.blockScene.Instantiate() as Block;
		_currentBlock.Initialize(blockResources[t]);

		_ghostBlock = _spawner.blockScene.Instantiate() as Block;
		_ghostBlock.Initialize(blockResources[t]);
		_ghostBlock.SetGhost(_ghostTexture);

		blockPosition = new Vector2(4, 0) + gridSpawnPositions[t];
		_currentBlock.Position = _grid.Position + (blockPosition * GRID_SIZE);

		SetGhostPosition();
		AddChild(_ghostBlock);
		AddChild(_currentBlock);

		_nextGrid.Populate();
	}

	public override void _Process(double delta)
	{
		if (_countdown.Started) return;

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
					swapped = false;
					SpawnNewBlock(_spawner.GetNextBlock());
					//miniFields[0].Populate(GetGrid());
					_root.Connection.SyncField(GetGrid());
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

		if (processControls)
		{
			//if(Input. )
			if (Gamepad.UpPressed())
			{
				Rotate(RIGHT);
				rotateSound.Play();
			}
			if (Gamepad.LeftPressed())
			{
				Move(LEFT);
				moveSound.Play();
			}
			else if (Gamepad.RightPressed())
			{
				Move(RIGHT);
				moveSound.Play();
			}
			else if (Gamepad.PressedB())
			{
				FastDrop();
				dropSound.Play();
			}
			else if (Gamepad.PressedA())
			{
				SwapBlocks();
				holdSound.Play();
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
					moveSound.Play();
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
	}

	public void Rotate(Vector2 direction)
	{
		int orientation = _currentBlock.GetOrientation();
		_currentBlock.Rotate(direction);
		int test_orientation = _currentBlock.GetOrientation();

		if (CheckCollisions(new Vector2(0, 0))) //colliding with something here, see if we can fix it!
		{
			int wall_kick_index = test_orientation * 2;
			if (direction == LEFT) wall_kick_index--;
			wall_kick_index = wrap(wall_kick_index, 8);
			List<Vector2> kicks;
			if (_currentBlock.Block_Type == BlockType.I) kicks = wallKicksI[wall_kick_index];
			else kicks = wallKicks[wall_kick_index];
			for (int i = 0; i < 4; i++)
			{
				if (!CheckCollisions(kicks[i]))
				{
					Move(kicks[i]);
					SetGhostPosition();
					return;
				}
			}
			_currentBlock.SetOrientation(orientation); //couldn't fix, set back to original orientation

		}
		SetGhostPosition();
	}

	public int wrap(int value, int max)
	{
		if (value < 0) value += max;
		if (value >= max) value += max;
		return value;
	}

	public void Move(Vector2 direction)
	{
		if (CheckCollisions(direction) == false)
		{
			blockPosition += direction;
			_currentBlock.Position = _grid.Position + (blockPosition * GRID_SIZE);
			SetGhostPosition();

			for (int i = 0; i < 4; i++)
			{
				Vector2 testPosition = blockPosition + _currentBlock.cells[i];
				int x = (int)testPosition.X;
				int y = (int)testPosition.Y;
				//GD.Print(testPosition);
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

	public void FastDrop()
	{
		Vector2 oldPosition = blockPosition;
		while (CheckCollisions(DOWN) == false)
		{
			Move(DOWN);
		}
		Vector2 newPosition = blockPosition;
		if (newPosition.Y - oldPosition.Y != 0)
		{
			List<Vector2> topPieceLocations = _currentBlock.GetTopPieces();
			foreach (Vector2 v in topPieceLocations)
			{
				Gradient g = _gradientScene.Instantiate() as Gradient;
				AddChild(g);
				g.Position = _grid.Position + (oldPosition + v) * 48;
				g.Size = new Vector2(48, (newPosition.Y - oldPosition.Y) * 48);
				//g.SetDeferred("Size", new Vector2(48, (newPosition.Y - oldPosition.Y) * 48));
				GD.Print(g.Size);
			}
		}
		Land();
	}

	public void SwapBlocks()
	{
		if (swapped) return;

		RemoveChild(_ghostBlock);
		if (!_holdGrid.HasBlock())
		{
			RemoveChild(_currentBlock);
			_holdGrid.SetBlock(_currentBlock);
			SpawnNewBlock(_spawner.GetNextBlock());
		}
		else
		{
			Block t = _currentBlock;
			RemoveChild(_currentBlock);
			SpawnNewBlock(_holdGrid.GetBlock().Block_Type);
			_holdGrid.SetBlock(t);
		}
		swapped = true;
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

	public void SetGhostPosition()
	{
		ghostPosition = blockPosition;
		_ghostBlock.SetOrientation(_currentBlock.GetOrientation());
		GhostDrop();
		_ghostBlock.Position = _grid.Position + (ghostPosition * GRID_SIZE);
	}

	public void GhostDrop()
	{
		while (CheckGhostCollisions(DOWN) == false)
		{
			ghostPosition += DOWN;
		}
	}

	public bool CheckGhostCollisions(Vector2 direction)
	{
		for (int i = 0; i < 4; i++)
		{
			Vector2 testPosition = ghostPosition + direction + _ghostBlock.cells[i];
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
		landSound.Play();
		swapped = false;
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
		RemoveChild(_ghostBlock);
		CheckLines();
		if (_lines.Count == 0) SpawnNewBlock(_spawner.GetNextBlock());

		//miniFields[0].Populate(GetGrid());

		_root.Connection.SyncField(GetGrid());
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

		if (numlines > 0)
		{
			if (numlines == 1) clearSound.Play();
			else if (numlines == 2 || numlines == 3) clear2Sound.Play();
			else clear3Sound.Play();
		}
		totalLines += numlines;

		_lblLines.Text = $"Lines - {totalLines}";
	}

	public sbyte[,] GetGrid()
	{
		sbyte[,] data = new sbyte[GRID_W, GRID_H];

		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{
				if (_gridData[x, y] != null)
				{
					Piece p = _gridData[x, y];
					data[x, y] = p.colorIndex;
				}
				else data[x, y] = -1;
			}
		}
		return data;
	}

	public void SetGrid(int[,] data, int gridIndex)
	{

	}

}
