using Godot;
using System;

/// <summary>
/// Main starting menu
/// </summary>
public class MainMenu : VBoxContainer, IScene
{
	public override void _Ready()
	{
		GetNode<Button>("Open").Connect("pressed", this, "Open");
		GetNode<Button>("Create").Connect("pressed", this, "Create");
	}

	private void Open()
	{
		SceneManager.Instance.LoadScene(SceneManager.Scene.StudysetsBrowser);
	}

	private void Create()
	{
		SceneManager.Instance.currentStudyset = new StudySet();
		SceneManager.Instance.LoadScene(SceneManager.Scene.StudysetEditor);
	}

	public SceneManager.Scene thisScene { get { return SceneManager.Scene.MainMenu; } }
	public void LoadStudyset(StudySet studySet)
	{
		SceneManager.Instance.currentStudyset = null;
	}
}
