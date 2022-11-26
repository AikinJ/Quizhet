using Godot;
using System;

public class CharacterButton : Button
{
	public WritingTest writingTest;

	public void pressed()
	{
		writingTest.CharacterButtonPressed(Text[0]);
	}

	public override void _Process(Single delta)
	{
		if (Input.IsKeyPressed((int)KeyList.Shift))
		{
			Text = Text.ToUpper();
		}
		else
		{
            Text = Text.ToLower();
		}
	}
}
