namespace TetraNet;

using System.Diagnostics;
using Godot;
using static Data;

public partial class Piece : Area2D
{
	[Export] private Sprite2D _sprite;
	[Export] private CollisionShape2D _collision;
	private Texture2D _ghostTexture;
	public int textureIndex;
	private MainField _main;

	public double flyTime = 0f;
	public double flashtime = 0f;
	public Vector2 velocity;

	public bool isItem = false;
	public ItemType itemType;

	public Color flyColor;

	public string mode;

	public void SetTexture(Texture2D texture)
	{
		_sprite.Texture = texture;
	}

	public void SetItem(ItemType i, Texture2D texture)
	{
		isItem = true;
		itemType = i;
		SetTexture(texture);
		textureIndex = (int)itemType + 7;
	}

	public Vector2 GetSize()
	{
		return _collision.Shape.GetRect().Size;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
		flyColor = new Color(1f, 1f, 1f);
	}

	public void Fly(MainField m)
	{
		_main = m;
		mode = "Fly";
		ZIndex++;
		float xv = GD.Randf() * 2 + 0.5f;
		if (GD.Randf() >= 0.5f) xv *= -1;
		float yv = -2f - GD.Randf() * 2;
		velocity = new Vector2(xv, yv);
		SetProcess(true);
	}

	public void Flash(MainField m)
	{
		_main = m;
		mode = "Flash";
		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (mode == "Fly")
		{
			ProcessFly(delta);
		}
		else if (mode == "Flash")
		{
			ProcessFlash(delta);
		}
	}

	public void ProcessFly(double delta)
	{

		flyTime += delta;
		if (flyTime < 1f)
		{
			Position += velocity;
			velocity.Y += 15f * (float)delta;
			RotationDegrees += velocity.X * 80 * (float)delta;
			flyColor.A = 1f - (float)flyTime;
			Modulate = flyColor;
		}
		else
		{
			_main.PieceDone(this);
			Hide();
			SetProcess(false);
		}
	}

	public void ProcessFlash(double delta)
	{
		flashtime += delta;
		if (flashtime < 0.1f)
		{
			float f = (float)delta;
			Color c = new Color(25f * f, 25f * f, 25f * f);
			Modulate += c;
		}
		else if (flashtime < 0.15f)
		{
			Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 1f - ((float)flashtime - 0.1f) * 20);
		}
		else
		{
			_main.PieceDone(this);
			Hide();
			SetProcess(false);
		}
	}

}
