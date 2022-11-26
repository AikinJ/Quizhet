using Godot;
using System;

public class FlashcardInOverview : HBoxContainer
{
	Label frontSideOfFlashcard;
	Label backSideOfFlashcard;

	public override void _Ready()
	{
		frontSideOfFlashcard = GetNode<Label>("PanelContainer/Label");
		backSideOfFlashcard = GetNode<Label>("PanelContainer2/Label2");
	}

	public void LoadFlashcard(Flashcard flashcard)
	{
		frontSideOfFlashcard.Text = flashcard.frontSide;
		backSideOfFlashcard.Text = flashcard.backSide;
	}
}
