using Godot;
using System;
using System.Collections.Generic;

public class SceneChangingButton : Button
{
	/// <summary>
	/// Scene to load when this button is pressed
	/// </summary>
	[Export] public SceneManager.Scene Scene = 0;

	public override void _Pressed()
	{
		SceneManager.Instance.LoadScene(Scene);
	}
}
