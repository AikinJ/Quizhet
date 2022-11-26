using Godot;
using System;
using System.Collections.Generic;

public class StudysetEditor : ScrollContainer, IScene
{
	public static StudysetEditor studysetEditor;
	Button addButton;
	PackedScene flashcardInEditorScene;
	VBoxContainer vboxContainer;

	LineEdit TitleLinedit;
	TextEdit DescriptionEdit;
	public override void _Ready()
	{
		studysetEditor = this;
		vboxContainer = GetNode<VBoxContainer>("VBoxContainer");
		addButton = GetNode<Button>("VBoxContainer/Button");
		addButton.Connect("pressed", this, "AddButtonPressed");
		TitleLinedit = GetNode<LineEdit>("VBoxContainer/PanelContainer/HBoxContainer/LineEdit");
		DescriptionEdit = GetNode<TextEdit>("VBoxContainer/PanelContainer/HBoxContainer/TextEdit");
		flashcardInEditorScene = GD.Load<PackedScene>("res://Prefabs/FlashcardInEditor.tscn");
		TitleLinedit.Connect("text_changed", this, "NameChanged");
		DescriptionEdit.Connect("text_changed", this, "DescriptionChanged");
	}

	public void LoadStudyset(StudySet studySet)
	{
		loadedStudySet = studySet;

		for (int i = 0; i < loadedStudySet.flashcards.Count; i++)
		{
			AddFlashCard(studySet.flashcards[i]);
		}
		vboxContainer.RemoveChild(addButton);
		vboxContainer.AddChild(addButton);
		TitleLinedit.Text = studySet.name;
		DescriptionEdit.Text = studySet.description;

	}

	StudySet loadedStudySet;
	List<FlashcardInEditor> loadedFlashcards = new List<FlashcardInEditor>();

	public SceneManager.Scene thisScene { get { return SceneManager.Scene.StudysetEditor; } }

	private void AddButtonPressed()
	{
		AddFlashCard(loadedStudySet.AddEmptyFlashcard());
		vboxContainer.RemoveChild(addButton);
		vboxContainer.AddChild(addButton);
	}

	private void RemoveFlashcard(int index)
	{
		loadedStudySet.RemoveFlashcard(index);
		FlashcardInEditor flashcardbeingdeleted = loadedFlashcards[index];
		loadedFlashcards.RemoveAt(index);
		flashcardbeingdeleted.QueueFree();
		ReloadFlashcardsFrom(index);
	}

	private void ReloadFlashcardsFrom(int index)
	{
		if(loadedFlashcards.Count != loadedStudySet.flashcards.Count)
		{
			GD.Print($"Error: {loadedFlashcards.Count} loaded cards and loaded studyset is {loadedStudySet.flashcards.Count} cards long");
			return;
		}
		for(int i = index;i< loadedFlashcards.Count;i++)
		{
			loadedFlashcards[i].LoadFlashcard(loadedStudySet.flashcards[i]);
		}
	}

	private void AddFlashCard(Flashcard flashcard)
	{
		FlashcardInEditor flashcardnode = flashcardInEditorScene.Instance<FlashcardInEditor>();
		vboxContainer.AddChild(flashcardnode);
		flashcardnode.LoadFlashcard(flashcard);
		flashcardnode.OnDelete += RemoveFlashcard;
		loadedFlashcards.Add(flashcardnode);
	}

	private void DescriptionChanged()
	{
		loadedStudySet.description = DescriptionEdit.Text;
		loadedStudySet.Save();
	}

	private void NameChanged(string text)
	{
		loadedStudySet.name = TitleLinedit.Text;
		loadedStudySet.Save();
	}


}
