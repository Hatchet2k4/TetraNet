namespace TetraNet;

using Godot;
using static Data;

public partial class Piece : Area2D
{
	[Export] private Sprite2D _sprite;
	[Export] private CollisionShape2D _collision;
	private Texture2D _ghostTexture;
	public sbyte colorIndex;
	private MainField _main;

	public double flyTime = 0f;
	public Vector2 velocity;

	public bool isItem = false;
	public ItemType itemType;

	public Color flyColor;

	public void SetTexture(Texture2D texture)
	{
		_sprite.Texture = texture;
	}

	public void SetItem(ItemType i, Texture2D texture)
	{
		isItem = true;
		itemType = i;
		SetTexture(texture);
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
		SetProcess(true);
		ZIndex++;
		float xv = GD.Randf() * 2 + 0.5f;
		if (GD.Randf() >= 0.5f) xv *= -1;
		float yv = -1f - GD.Randf();
		velocity = new Vector2(xv, yv);
	}

	public override void _Process(double delta)
	{
		flyTime += delta;
		if (flyTime < 1f)
		{
			Position += velocity;
			velocity.Y += 10f * (float)delta;
			RotationDegrees += velocity.X * 80 * (float)delta;
			Modulate = new Color(flyColor.R, flyColor.G, flyColor.B, 1f - (float)flyTime);
		}
		else
		{
			_main.PieceDone(this);
			SetProcess(false);
		}
	}

}
