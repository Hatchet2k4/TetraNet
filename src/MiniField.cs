namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using static Data;

public partial class MiniField : Control
{
	[Export] private Label _nameLabel;
	[Export] private TextureRect _grid;
	[Export] private Texture2D[] _textures;

	private List<Piece> _pieces;
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");


	public override void _Ready()
	{
		_pieces = new();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetName(string name)
	{
		_nameLabel.Text = name;
	}

	private void ClearPieces()
	{
		foreach (Piece p in _pieces)
		{
			RemoveChild(p);
		}
		_pieces.Clear();
	}
	public void Populate(sbyte[,] data)
	{
		ClearPieces();
		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{
				if (data[x, y] > 0)
				{
					Piece p = _pieceScene.Instantiate() as Piece;
					AddChild(p);
					p.Position = _grid.Position + new Vector2(x * GRID_SIZE, y * GRID_SIZE);
					p.SetTexture(_textures[data[x, y]]);
					p.ZIndex = 2;
					_pieces.Add(p);
				}
			}
		}
	}

}
