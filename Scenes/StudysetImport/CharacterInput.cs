using Godot;
using System;

/// <summary>
/// Node that is made to return a single character, used to divide cards or fronts of cards
/// </summary>
public abstract class CharacterInput : PanelContainer
{
	abstract public char checkboxCharacter();
	
	private CheckBox checkBox;
	private LineEdit lineEdit;
	private bool checkbox = false;

	public override void _Ready()
	{
		checkBox = GetNode<CheckBox>("HBoxContainer/CheckBox");
		lineEdit = GetNode<LineEdit>("HBoxContainer/LineEdit");
		checkBox.Connect("toggled", this, "CheckboxToggled");
	}

	public char GetCharacter()
	{
		if (checkbox || lineEdit.Text.Length == 0)
		{
			return checkboxCharacter();
		}
		else  return lineEdit.Text[0];
	}

	private void CheckboxToggled(bool toggle)
	{
		checkbox = toggle;
		lineEdit.Visible = !toggle;
	}
}
