using Godot;
using System;

public class ImportStudyset : VBoxContainer, IScene
{
	TextEdit textInput;
	Button startImportButton;
	CharacterInput betweenCards;
	CharacterInput betweenFrontAndBack;

	StudySet openedStudyset;

	public SceneManager.Scene thisScene { get { return SceneManager.Scene.ImportStudyset; } }

	public override void _Ready()
	{
		textInput = GetNode<TextEdit>("TextEdit");
		startImportButton = GetNode<Button>("HBoxContainer/Button");
		betweenCards = GetNode<CharacterInput>("HBoxContainer/PanelContainer");
		betweenFrontAndBack = GetNode<CharacterInput>("HBoxContainer/PanelContainer2");
		startImportButton.Connect("pressed", this, "Test");

	}

	public StudySet ImportStudySet()
	{
		return new StudySet(textInput.Text, betweenCards.GetCharacter(), betweenFrontAndBack.GetCharacter());
	}

	private void Test()
	{
		StudySet studySet = ImportStudySet();
		int length = openedStudyset.flashcards.Count;
		openedStudyset.flashcards.AddRange(studySet.flashcards);
		openedStudyset.ResetIndexFrom(length);
		openedStudyset.Save();
		SceneManager.Instance.LoadScene(SceneManager.Scene.StudysetEditor);

	}

	public void LoadStudyset(StudySet studySet)
	{
		openedStudyset = studySet;
	}
}
