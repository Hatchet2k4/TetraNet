namespace TetraNet;

using Godot;
using static Data;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;
using System.Reflection;



public partial class MainField : Control
{

	private List<Piece> _processingPieces = new();
	public void ProcessActionQueue()
	{
		foreach (ItemType it in _actionQueue)
		{
			//defense
			if (it == ItemType.C) ClearLineAction();
			else if (it == ItemType.D) DefenseAction();
			else if (it == ItemType.G) GravityAction();
			else if (it == ItemType.N) NukeAction();
			else if (it == ItemType.A) AddLineAction();
			else if (it == ItemType.B) BlockClearAction();
			else if (it == ItemType.L) LockRotationAction();
			else if (it == ItemType.H) LockHoldAction();
			else if (it == ItemType.O) BlockBombAction();
			else if (it == ItemType.Q) QuakeAction();
			else if (it == ItemType.R) RandomAction();
			else if (it == ItemType.S) SwitchAction();
		}
		_actionQueue.Clear();

	}

	public void AddLineAction()
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
				AddFieldPiece(x, GRID_H - 1, _blackTexture, 19); //19 texture is black
			}
		}
		addLineSound.Play();
	}

	public void ClearLineAction()
	{
		for (int x = 0; x < GRID_W; x++)
		{
			if (_gridData[x, GRID_H - 1] != null)
			{
				RemoveChild(_gridData[x, GRID_H - 1]);
				_gridData[x, GRID_H - 1] = null;
			}
			for (int dy = GRID_H - 1; dy >= 0; dy--)
			{
				if (dy > 0) _gridData[x, dy] = _gridData[x, dy - 1];
				else _gridData[x, dy] = null;
			}
		}
		clearLineSound.Play();
	}

	public void GravityAction()
	{
		for (int x = 0; x < GRID_W; x++)
		{
			SortColumn(x);
		}
		gravitySound.Play();
	}

	public void SortColumn(int x)
	{
		for (int y = GRID_H - 1; y > 1; y--)
		{
			if (_gridData[x, y] is null)
			{
				for (int dy = y - 1; dy > 1; dy--)
				{
					if (_gridData[x, dy] is not null)
					{
						MovePiece(x, dy, x, y);
						break;
					}
				}
			}
		}
	}

	public void BlockBombAction()
	{
		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{
				if (_gridData[x, y] != null)
				{
					Piece p = _gridData[x, y];
					if (p.isItem && p.itemType == ItemType.O)
					{
						Explode(x, y, p);
					}
					_processingPieces.Add(_gridData[x, y]);
					_gridData[x, y].Flash(this);
					_gridData[x, y] = null;
				}
			}
		}
	}

	public void Explode(int x, int y, Piece p)
	{
		List<Vector2> checkList = new()
		{
			new Vector2(-1, -1),
			new Vector2(0, -1),
			new Vector2(1, -1),
			new Vector2(-1, 0),
			new Vector2(1, 0),
			new Vector2(-1, 1),
			new Vector2(0, 1),
			new Vector2(1, 1)
		};

		foreach (Vector2 position in checkList)
		{
			if (InBounds(x, y) && _gridData[x, y] != null)
			{

			}
		}
	}

	public bool InBounds(int x, int y)
	{
		return x >= 0 && x < GRID_W && y >= 0 && y < GRID_H;
	}

	public void BlockClearAction()
	{

	}

	public void NukeAction()
	{
		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{

				if (_gridData[x, y] != null)
				{
					_processingPieces.Add(_gridData[x, y]);
					_gridData[x, y].Flash(this);
					_gridData[x, y] = null;
				}
			}
		}
		nukeSound.Play();
	}

	public void QuakeAction()
	{
		for (int y = 0; y < GRID_H; y++)
		{
			int shiftAmount = _rand.Next(-2, 3); // Random shift amount between -2 and 2
			ShiftRow(y, shiftAmount);
		}
	}

	private void ShiftRow(int rowIndex, int shiftAmount)
	{
		Piece[] newRow = new Piece[GRID_W];

		for (int x = 0; x < GRID_W; x++)
		{
			// Calculate the new position after shifting, wrapping around if necessary
			int newPosition = (x + shiftAmount + GRID_W) % GRID_W;
			newRow[newPosition] = _gridData[x, rowIndex];
		}
		// Copy the shifted row back to the grid
		for (int x = 0; x < GRID_W; x++)
		{
			_gridData[x, rowIndex] = newRow[x];
		}
	}

	public void RandomAction()
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++) //10 chances to try and find a non-null block
			{
				int x = _rand.Next(GRID_W);
				int y = _rand.Next(GRID_H);
				if (_gridData[x, y] != null)
				{
					RemoveChild(_gridData[x, y]);
					_gridData[x, y] = null;
					break;
				}
			}
		}
	}

	public void DefenseAction()
	{

	}

	public void LockRotationAction()
	{

	}

	public void LockHoldAction()
	{

	}

	public void SwitchAction()
	{

	}


	public void CheckLinesAction()
	{
		int numlines = 0;
		List<int> lines = new List<int>();
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
					lines.Add(y); //found on line y
				}
			}
		}

		//clear found lines
		for (int i = 0; i < lines.Count; i++)
		{
			for (int x = 0; x < GRID_W; x++)
			{
				int y = lines[i];
				if (_gridData[x, y] != null)
				{
					Piece p = _gridData[x, y];
					p.flyColor = new Color(0.2f, 0.2f, 1f, 1f);
					p.Fly(this);
					_processingPieces.Add(p);
					_gridData[x, y] = null;
				}
			}
		}

		//clear blank lines
		foreach (int y in lines)
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
		ResetPiecePositions();
		GhostDrop();
	}
}
