namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;



public partial class MainField : Control
{
	[Export] private Main _root;
	[Export] private Label _playerName;
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
	[Export] private AudioStreamPlayer gameOverSound;
	[Export] public Texture2D[] itemTextures;
	[Export] private Inventory _inventory;
	private Random _rand;
	public List<MiniField> miniFields;
	private Texture2D _ghostTexture;
	private Texture2D _blackTexture;

	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");

	private PackedScene _minifieldScene = (PackedScene)ResourceLoader.Load("res://scenes/mini_field.tscn");

	private PackedScene _gradientScene = (PackedScene)ResourceLoader.Load("res://scenes/gradient.tscn");

	private double _fallTime;

	private Piece[,] _gridData;
	private List<Piece> _flyingPieces = new();

	private bool _gameOver;
	private float _gameOverTime;
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

	private List<ItemType> _actionQueue = new();


	public override void _Ready()
	{
		_rand = new();
		_ghostTexture = GD.Load("res://gfx/Ghost.png") as Texture2D;
		_blackTexture = GD.Load("res://gfx/Black.png") as Texture2D;
		_fallTime = 0.5f;
		_time = 0;
		_gameOverTime = 0f;
		_gridData = new Piece[GRID_W, GRID_H];
		_lines = new();
		miniFields = new();
		SetProcess(false);
	}

	public void SetName(string name)
	{
		_playerName.Text = name;
	}

	public void CreateMiniFields()
	{
		float ypos = 60;
		int index = 0;
		for (int x = 0; x < 6; x++)
		{
			MiniField mf = _minifieldScene.Instantiate() as MiniField;
			_root.CallDeferred("add_child", mf);
			float xpos = 874 + (256 + 24) * x;
			mf.Position = new Vector2(xpos, ypos);
			mf.SetName($"({index + 1})", "");
			miniFields.Add(mf);
			index++;
		}
		ypos = 772;
		for (int x = 1; x < 6; x++)
		{
			MiniField mf = _minifieldScene.Instantiate() as MiniField;
			_root.CallDeferred("add_child", mf);
			float xpos = 874 + (256 + 24) * x;
			mf.Position = new Vector2(xpos, ypos);
			mf.SetName($"({index + 1})", "");
			miniFields.Add(mf);
			index++;
		}

		index = 0;
		foreach (long id in _root.gameData.PlayerList.Keys)
		{
			if (id != _root.gameData.Id)
			{
				_root.gameData.fieldMappings[id] = index;
				miniFields[index].SetName(_root.gameData.PlayerList[id].PlayerName);
				index++;
			}
		}

	}

	public void NewGame()
	{
		CreateMiniFields();
		swapped = false;
		totalLines = 0;
		SpawnNewBlock(_spawner.GetNextBlock());
		//_countdown.Start();
		music.Play();
		SetProcess(true);
	}

	public void SpawnNewBlock(BlockType t)
	{
		_currentBlock = _spawner.blockScene.Instantiate() as Block;
		_currentBlock.Initialize(blockResources[t]);
		blockPosition = new Vector2(4, 0) + gridSpawnPositions[t];
		_currentBlock.Position = _grid.Position + (blockPosition * GRID_SIZE);
		AddChild(_currentBlock);
		_nextGrid.Populate();

		if (CheckCollisions(new Vector2(0, 0))) //uh oh! 
		{
			GameOver();
			return;
		}

		_ghostBlock = _spawner.blockScene.Instantiate() as Block;
		_ghostBlock.Initialize(blockResources[t]);
		_ghostBlock.SetGhost(_ghostTexture);
		SetGhostPosition();
		AddChild(_ghostBlock);


	}

	public void GameOver()
	{
		_gameOver = true;
		processControls = false;
		music.Stop();
		gameOverSound.Play();
		foreach (Piece p in _gridData)
		{
			if (p != null)
			{
				p.Fly(this);
				_flyingPieces.Add(p);
			}
		}
		foreach (Piece p in _currentBlock.pieces)
		{
			p.Fly(this);
			_currentBlock.RemoveChild(p);
			AddChild(p);
			_flyingPieces.Add(p);
		}
	}

	public override void _Process(double delta)
	{
		if (_countdown.Started) return;

		if (_gameOver)
		{
			_gameOverTime += (float)delta;
			if (_gameOverTime >= 3f)
			{
				GD.Print("gameover timeout");
				_root.StopGame();
			}

			return;
		}

		if (_lines.Count > 0) //processing lines
		{
			ClearLines(delta);
			return; //may refactor this later to allow inventory input even while lines are being cleared
		}

		if (_actionQueue.Count > 0)
		{
			ProcessActionQueue();
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

	public void ProcessActionQueue()
	{
		foreach (ItemType it in _actionQueue)
		{
			if (it == ItemType.A) AddLine();
		}
		_actionQueue.Clear();
		if (_root.connection.Mode != ConnectionMode.None) _root.connection.SyncField(_root.gameData.Id, GetGrid());
	}

	public void AddFieldPiece(int x, int y, Texture2D texture)
	{
		Piece p = _pieceScene.Instantiate() as Piece;
		p.SetTexture(texture);
		p.Position = _grid.Position + new Vector2(x * GRID_SIZE, y * GRID_SIZE);
		_gridData[x, GRID_H - 1] = p;
		AddChild(p);
	}

	public void MovePiece(int x1, int y1, int x2, int y2)
	{
		_gridData[x2, y2] = _gridData[x1, y1];
		if (_gridData[x2, y2] != null)
		{
			_gridData[x2, y2].Position = _grid.Position + new Vector2(x2 * GRID_SIZE, y2 * GRID_SIZE);
		}
		_gridData[x1, y1] = null;
	}

	public void AddLine()
	{
		for (int x = 0; x < GRID_W; x++)
		{
			if (_gridData[x, 0] != null) //piece in top row
			{
				GameOver();
				return;
			}
		}
		for (int y = 1; y < GRID_H; y++)
		{
			for (int x = 0; x < GRID_W; x++)
			{
				MovePiece(x, y, x, y - 1);
			}
		}
		int hole = _rand.Next(10);
		for (int x = 0; x < GRID_W; x++)
		{
			if (x != hole)
			{
				AddFieldPiece(x, GRID_H - 1, _blackTexture);
			}
		}
	}

	public void ClearLines(double delta)
	{
		clearTimer += delta;
		if (clearTimer >= 0.02f)
		{
			clearTimer -= 0.02f;
			for (int i = 0; i < _lines.Count; i++)
			{
				int y = _lines[i];
				Piece p = _gridData[clearIndex, y];
				if (p.isItem)
				{
					_inventory.AddItem(p.itemType);
				}
				p.Fly(this);
				_flyingPieces.Add(p);
				_gridData[clearIndex, y] = null;
			}
			clearIndex++;
			if (clearIndex == GRID_W)
			{
				RemoveBlankLines();
				ResetPiecePositions();

				SpawnItems(_lines.Count);

				_lines.Clear();
				clearTimer = 0f;
				clearIndex = 0;
				swapped = false;
				SpawnNewBlock(_spawner.GetNextBlock());
				if (_root.connection.Mode != ConnectionMode.None) _root.connection.SyncField(_root.gameData.Id, GetGrid());
			}
		}
	}

	public void PieceDone(Piece p)
	{
		_flyingPieces.Remove(p);
		RemoveChild(p);
	}

	public void SpawnItems(int count)
	{
		for (int i = 0; i < count; i++)
		{
			int itemnum = _rand.Next(0, NUM_ITEMS); //todo - item weights
			Piece p = GetRandomPiece();
			if (p != null)
			{
				p.SetItem((ItemType)itemnum, itemTextures[itemnum]);
			}
		}
	}

	public Piece GetRandomPiece()
	{
		int x;
		int y;
		for (int i = 0; i < 80; i++) //80 chances to find a random piece
		{
			x = _rand.Next(0, GRID_W);
			y = _rand.Next(0, GRID_H);
			try
			{
				Piece p = _gridData[x, y];
				if (p != null)
				{
					if (!p.isItem) return p;
				}
			}
			catch
			{
				GD.Print($"X: {x}  Y: {y}");
			}
		}
		return null;
	}

	public void ProcessInput(double delta)
	{

		if (processControls)
		{
			if (Gamepad.PressedY())
			{
				_actionQueue.Add(ItemType.A);
			}

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

		if (_root.connection.Mode != ConnectionMode.None) _root.connection.SyncField(_root.gameData.Id, GetGrid());
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

	public sbyte[] GetGrid()
	{
		sbyte[] data = new sbyte[GRID_W * GRID_H];

		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{
				if (_gridData[x, y] != null)
				{
					Piece p = _gridData[x, y];
					data[y * GRID_W + x] = p.colorIndex;
				}
				else data[y * GRID_W + x] = -1;
			}
		}
		return data;
	}

	public void SetGrid(int[,] data, int gridIndex)
	{

	}

}
