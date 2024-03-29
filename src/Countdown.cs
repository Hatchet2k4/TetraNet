using Godot;
using System;

public partial class Countdown : TextureRect
{
	[Export] private Texture2D[] Numbers;
	[Export] private AudioStreamPlayer beepSound;
	[Export] private AudioStreamPlayer doneSound;

	private float count;
	public bool Started = false;
	private int curdigit;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		count += (float)delta;
		if (Started)
		{
			if (count < 3f)
			{
				int digit = (int)Math.Truncate(count);
				float fraction = count - digit;
				Modulate = new Color(1f, 1f, 1f, 1f - fraction);
				Scale = new Vector2(2 - fraction, 2 - fraction);
				if (digit > curdigit)
				{
					curdigit++;
					Texture = Numbers[digit];
					beepSound.Play();
				}
			}
			else
			{
				Started = false;
				doneSound.Play();
				Hide();
			}

		}
	}

	public void Start()
	{
		Texture = Numbers[0];
		count = 0f;
		curdigit = 0;
		Started = true;
		beepSound.Play();
		Show();
	}
}
