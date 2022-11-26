using Godot;
using System;

public class TextInputImporter : TextEdit
{
	public override void _Ready()
	{
		Connect("text_changed", this, "OnTextChanged");
	}

	private void OnTextChanged()
	{

	}
}
