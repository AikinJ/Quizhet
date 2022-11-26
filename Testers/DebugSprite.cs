using Godot;
using System;

public class DebugSprite : Sprite
{
	static DebugSprite instance;

	public override void _Ready()
	{
		instance = this; 
	}

	public static void MoveTo(Vector2 vector2)
	{
		//instance.GlobalPosition = vector2;
	}
}
