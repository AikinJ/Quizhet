using Godot;
using System;

public class StudysetsBrowserButton : Button
{
	public override void _Ready()
	{
		Connect("pressed", this, "Pressed");
		Align = (TextAlign.Left);
	}

	private void Pressed()
	{
		SceneManager.Instance.currentStudyset = StudySet.LoadStudysetFromFile(Prefs.currentStudysetsPath + '\\' + Text + StudySet.fileextension);
		SceneManager.Instance.LoadScene(SceneManager.Scene.StudysetOverview);
	}
}
