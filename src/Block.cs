namespace TetraNet;

using Godot;
using System.Collections.Generic;
using System.Linq;
using static Data;

public partial class Block : Node2D
{
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");
	private BlockData _blockData;
	public Piece[] pieces;
	public Shape shape;
	public List<Vector2> cells;

	public BlockType Block_Type
	{
		get
		{
			return _blockData.blockType;
		}
	}

	public override void _Ready()
	{
		SetProcess(false);
	}

	public void Initialize(BlockData bd)
	{
		_blockData = bd;
		pieces = new Piece[4];
		shape = new Shape(_blockData.blockType);
		cells = shape.GetCells();

		for (int i = 0; i < 4; i++)
		{
			Piece p = _pieceScene.Instantiate() as Piece;
			AddChild(p);
			p.Position = cells[i] * GRID_SIZE;
			p.SetTexture(bd.blockTexture);
			p.ZIndex = 2;
			p.textureIndex = colorIndexMapping[_blockData.blockType];
			pieces[i] = p;
		}
	}

	public List<Vector2> GetTopPieces()
	{
		Dictionary<float, Vector2> highestPieces = new Dictionary<float, Vector2>();

		foreach (Vector2 position in cells)
		{
			if (highestPieces.ContainsKey(position.X))
			{
				if (position.Y > highestPieces[position.X].Y)
				{
					highestPieces[position.X] = position;
				}
			}
			else
			{
				highestPieces.Add(position.X, position);
			}
		}
		return new List<Vector2>(highestPieces.Values);
	}

	public void SetGhost(Texture2D t)
	{
		foreach (Piece p in pieces)
		{
			p.SetTexture(t);
		}
	}

	public void Rotate(Vector2 direction)
	{
		shape.Rotate(direction);
		cells = shape.GetCells();
		ResetPositions();
	}

	public void DefaultOrientation()
	{
		shape.orientation = 0;
		cells = shape.GetCells();
		ResetPositions();
	}

	public void SetOrientation(int orientation)
	{
		shape.orientation = orientation;
		cells = shape.GetCells();
		ResetPositions();
	}

	public int GetOrientation()
	{
		return shape.orientation;
	}

	public void ResetPositions()
	{
		for (int i = 0; i < 4; i++)
		{
			pieces[i].Position = cells[i] * GRID_SIZE;
		}
	}

	public Piece GetPiece(int index)
	{
		return pieces[index];
	}
}
