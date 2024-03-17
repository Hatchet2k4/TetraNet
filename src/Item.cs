using Godot;
using System;

public partial class Item : TextureRect
{
	[Export] private Label itemCount;
	private int _count;
	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			SetText();
		}
	}
	public int maxCount;

	public override void _Ready()
	{
		maxCount = 3;
		SetProcess(false);
	}

	public void SetText()
	{
		itemCount.Text = $"{_count} / {maxCount}";
	}
}
