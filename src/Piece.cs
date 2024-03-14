namespace TetraNet;

using Godot;

public partial class Piece : Area2D
{
	[Export] private Sprite2D _sprite;
	[Export] private CollisionShape2D _collision;
	private Texture2D _ghostTexture;
	public sbyte colorIndex;

	public void SetTexture(Texture2D texture)
	{
		_sprite.Texture = texture;
	}

	public Vector2 GetSize()
	{
		return _collision.Shape.GetRect().Size;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
	}
}
