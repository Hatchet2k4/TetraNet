namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using static Data;

public partial class MiniField : Control
{
	[Export] private RichTextLabel _nameLabel;
	[Export] private TextureRect _grid;
	[Export] private Texture2D[] _textures;

	[Export] private Label _linesLabel;
	[Export] private Label _koLabel;

	private List<Piece> _pieces;
	private PackedScene _pieceScene = (PackedScene)ResourceLoader.Load("res://scenes/piece.tscn");

	public override void _Ready()
	{
		_pieces = new();
	}

	public override void _Process(double delta)
	{
	}

	public void SetName(string name, string team = "")
	{
		_nameLabel.Text = $"[center]{name}[/center]";
	}

	private void ClearPieces()
	{
		foreach (Piece p in _pieces)
		{
			RemoveChild(p);
		}
		_pieces.Clear();
	}
	public void Populate(sbyte[] data)
	{
		ClearPieces();
		for (int x = 0; x < GRID_W; x++)
		{
			for (int y = 0; y < GRID_H; y++)
			{
				if (data[y * GRID_W + x] >= 0)
				{
					Piece p = _pieceScene.Instantiate() as Piece;
					AddChild(p);
					p.Position = _grid.Position + new Vector2(x * GRID_SIZE, y * GRID_SIZE);
					p.SetTexture(_textures[data[y * GRID_W + x]]);
					p.ZIndex = 2;
					_pieces.Add(p);
				}
			}
		}
	}

}
