using Godot;
using System;

public class StudysetOverview : VBoxContainer, IScene
{
	Label title;
	RichTextLabel description;
	PackedScene flashcard;
	StudySet currentStudyset;
	VBoxContainer flashcardsScrollfield;

	SceneManager.Scene IScene.thisScene { get { return SceneManager.Scene.StudysetOverview; } }

	public override void _Ready()
	{
		flashcard = GD.Load<PackedScene>("res://Prefabs/FlashcardInOverview.tscn");
		title = GetNode<Label>("PanelContainer/HBoxContainer/Label");
		description = GetNode<RichTextLabel>("PanelContainer/HBoxContainer/RichTextLabel");
		flashcardsScrollfield = GetNode<VBoxContainer>("PanelContainer2/VBoxContainer/ScrollContainer/VBoxContainer");
	}
	public void LoadStudyset(StudySet studySet)
	{
		currentStudyset = studySet;

		title.Text = studySet.name;
		description.Text = studySet.description;

		for(int i =0;i<studySet.flashcards.Count;i++)
		{
			FlashcardInOverview flashcardInEditor = (FlashcardInOverview)(flashcard.Instance());
			flashcardsScrollfield.AddChild(flashcardInEditor);
			flashcardInEditor.LoadFlashcard(studySet.flashcards[i]);
		}
	}
}
