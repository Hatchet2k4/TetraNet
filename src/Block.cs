namespace TetraNet;

using Godot;
using System.Collections.Generic;
using static Data;

public partial class Block : Node2D
{
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");
	private BlockData _blockData;
	private Piece[] _pieces;
	public Shape shape;
	public List<Vector2> cells;

	public BlockType BlockType
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
		_pieces = new Piece[4];
		shape = new Shape(_blockData.blockType);
		cells = shape.GetCells();

		for (int i = 0; i < 4; i++)
		{
			Piece p = _pieceScene.Instantiate() as Piece;
			p.Position = cells[i] * GRID_SIZE;
			p.SetTexture(bd.blockTexture);
			_pieces[i] = p;
			AddChild(p);
		}
	}

	public void Rotate(Vector2 direction)
	{
		shape.Rotate(direction);
		cells = shape.GetCells();
		ResetPositions();
	}

	public void ResetPositions()
	{
		for (int i = 0; i < 4; i++)
		{
			_pieces[i].Position = cells[i] * GRID_SIZE;
		}
	}

	public Piece GetPiece(int index)
	{
		return _pieces[index];
	}


}
