using Godot;
using System;

public class ButtonTest : Button
{
	public override void _Ready()
	{
		Connect("pressed", this, "OnPressed");
	}

	private void OnPressed()
	{
		string clipboard = "wat is een bij?#een insectje\nwat is honing?#een zoete substantie gemaakt door de bij\nwat is honing?#een zoete substantie gemaakt door de bij";

		StudySet studySet = new StudySet(clipboard, '\n', '#');
		foreach(Flashcard flashcard in studySet.flashcards)
		{
			GD.Print(flashcard);
		}
		WritingTest.writingTest.LoadStudyset(studySet);
		//StudysetEditor.studysetEditor.LoadStudySet(studySet);
	}
}
