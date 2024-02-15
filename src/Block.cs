namespace TetraNet;

using Godot;
using System.Collections.Generic;

public partial class Block : Node2D
{
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");
	private BlockData _blockData;
	private Piece[] _pieces;

	public override void _Ready()
	{
		SetProcess(false);
	}

	public void Initialize(BlockData bd)
	{
		_blockData = bd;
		_pieces = new Piece[4];
		List<Vector2> cells = Data.Cells[_blockData.blockType];

		for (int i = 0; i < 4; i++)
		{
			Piece p = _pieceScene.Instantiate() as Piece;
			p.Position = cells[i] * p.GetSize();
			p.SetTexture(bd.blockTexture);
			_pieces[i] = p;
			AddChild(p);
		}

	}

	public override void _Process(double delta)
	{
	}
}
