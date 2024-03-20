using Godot;
using System;
using TetraNet;

public partial class Item : TextureRect
{
	public Inventory inventory;
	[Export] private Label itemCount;
	private int _count;
	public int index;
	public double clickTimer;
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

	}

	public void SetText()
	{
		itemCount.Text = $"{_count} / {maxCount}";
	}

	public void GUIInput(InputEvent e)
	{
		if (e is InputEventMouseButton)
		{
			if (clickTimer > 0f)
			{
				inventory.UseItem();
			}
			else
			{
				InputEventMouseButton ev = (InputEventMouseButton)e;
				inventory.Target(index);
				clickTimer = 0.15f;
			}
		}
	}


	public override void _Process(double delta)
	{
		if (clickTimer > 0f) clickTimer -= delta;
		else clickTimer = 0f;

	}

}
