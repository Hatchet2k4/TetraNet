namespace TetraNet;

using Godot;
using System.Collections.Generic;
using static Data;

public partial class Block : Node2D
{
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");
	private BlockData _blockData;
	private Piece[] _pieces;
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
		cells = Data.Cells[_blockData.blockType];

		for (int i = 0; i < 4; i++)
		{
			Piece p = _pieceScene.Instantiate() as Piece;
			p.Position = cells[i] * p.GetSize();
			p.SetTexture(bd.blockTexture);
			_pieces[i] = p;
			AddChild(p);
		}
	}

	public Piece GetPiece(int index)
	{
		return _pieces[index];
	}


}
