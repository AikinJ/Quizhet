using Godot;
using System;

public class FlashcardInEditor : HBoxContainer
{
	/// <summary>
	/// Occurs when the remove button is pressed on the flashcard in the editor
	/// </summary>
	public Action<int> OnDelete;

	Label numberLabel;
	TextEdit frontSideOfFlashcard;
	TextEdit backSideOfFlashcard;
	Button removeButton;
	Flashcard loadedFlashcard;

	public override void _Ready()
	{
		//Get the children and assign them
		var children = GetChildren();
		numberLabel = (Label)children[0];
		frontSideOfFlashcard = (TextEdit)children[1];
		backSideOfFlashcard = (TextEdit)children[2];
		removeButton = (Button)children[3];
		//Connect the methods to the signals
		frontSideOfFlashcard.Connect("text_changed", this, "FrontsideChanged");
		backSideOfFlashcard.Connect("text_changed", this, "BacksideChanged");
		removeButton.Connect("pressed", this, "OnDeleteButtonPressed");
	}

	public void LoadFlashcard(Flashcard flashcard)
	{
		this.loadedFlashcard = flashcard;
		numberLabel.Text = loadedFlashcard.index.ToString();
		frontSideOfFlashcard.Text = loadedFlashcard.frontSide;
		backSideOfFlashcard.Text = loadedFlashcard.backSide;
	}

	private void FrontsideChanged()
	{
		loadedFlashcard.frontSide = frontSideOfFlashcard.Text;
	}

	private void BacksideChanged()
	{
		loadedFlashcard.backSide = backSideOfFlashcard.Text;
	}

	private void OnDeleteButtonPressed()
	{
		if (OnDelete != null) OnDelete(loadedFlashcard.index); 
	}
}
