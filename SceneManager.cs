using Godot;
using System;

public class SceneManager : Control
{
	public static SceneManager Instance { get; private set; }
	public StudySet currentStudyset;
	
	/// <summary>
	/// The main margin container, where all scenes are loaded into
	/// </summary>
	MarginContainer marginContainer;
	PackedScene[] scenes = new PackedScene[(int)Scene.Count];
	/// <summary>
	/// The node of the current scene
	/// </summary>
	Node currentSceneNode;
	Scene currentScene;

	public override void _Ready()
	{
		Instance = this;

		marginContainer = GetNode<MarginContainer>("MarginContainer");

		scenes[(int)Scene.MainMenu] = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn");
		scenes[(int)Scene.StudysetsBrowser] = GD.Load<PackedScene>("res://Scenes/StudysetsBrowser.tscn");
		scenes[(int)Scene.StudysetOverview] = GD.Load<PackedScene>("res://Scenes/StudysetOverview.tscn");
		scenes[(int)Scene.ImportStudyset] = GD.Load<PackedScene>("res://Scenes/ImportStudyset.tscn");
		scenes[(int)Scene.StudysetEditor] = GD.Load<PackedScene>("res://Scenes/StudysetEditor.tscn");
		scenes[(int)Scene.WritingTest] = GD.Load<PackedScene>("res://Scenes/WritingTest.tscn");

		LoadScene(Scene.MainMenu);
	}

	/// <summary>
	/// Occurs when back button is pressed
	/// </summary>
	public void Back()
	{
		if (currentScene == Scene.StudysetEditor)
		{
			LoadScene(Scene.StudysetOverview);
		}
		else if (currentScene == Scene.StudysetOverview)
		{
			LoadScene(Scene.StudysetsBrowser);
		}
		else if (currentScene == Scene.StudysetsBrowser)
		{
			LoadScene(Scene.MainMenu);
		}
		else if (currentScene == Scene.WritingTest)
		{
			LoadScene(Scene.StudysetOverview);
		}
	}

	public void LoadScene(Scene scene)
	{
		if (currentScene == Scene.StudysetEditor) currentStudyset.Save();
		if (currentSceneNode != null) currentSceneNode.QueueFree();
		currentSceneNode = scenes[(int)scene].Instance();
		marginContainer.AddChild(currentSceneNode);
		currentScene = scene;
		((IScene)currentSceneNode).LoadStudyset(currentStudyset);
	}

	public enum Scene
	{
		ImportStudyset,
		MainMenu,
		StudysetEditor,
		StudysetOverview,
		StudysetsBrowser,
		WritingTest,
		Count
	}
}

