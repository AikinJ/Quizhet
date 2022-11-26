using Godot;
using System;
using System.Collections.Generic;

public class StudysetsBrowser : PanelContainer, IScene
{

	Button studysetsPathButton;
	FileDialog studysetsFileDialog;
	VBoxContainer studysetsContainer;
	public override void _Ready()
	{
		studysetsFileDialog = GetNode<FileDialog>("FileDialog");
		studysetsFileDialog.Connect("dir_selected", this,"DirectorySelected");
		studysetsPathButton = GetNode<Button>("StudysetBrowser/HBoxContainer/Button");
		studysetsPathButton.Connect("pressed", this, "OpenFileBrowser");
		studysetsContainer = GetNode<VBoxContainer>("StudysetBrowser/ScrollContainer/VBoxContainer");

		(string, bool) prefPath = Prefs.GetPref(Prefs.studysetsPathPref);
		if(prefPath.Item2)
		{
			studysetsPathButton.Text = prefPath.Item1;
			LoadStudysets();
		}
	}

	public void OpenFileBrowser()
	{
		GD.Print("pressed");
		studysetsFileDialog.Popup_();
	}

	private void DirectorySelected(string path)
	{
		studysetsPathButton.Text = path;
		Prefs.ChangePref(Prefs.studysetsPathPref, path);
		LoadStudysets();

	}

	List<string> StudysetsInFolder;

	public SceneManager.Scene thisScene { get { return SceneManager.Scene.StudysetsBrowser; } }

	private void LoadStudysets()
	{
		StudysetsInFolder = new List<string>();
		Directory directory = new Directory();
		directory.Open(Prefs.currentStudysetsPath);
		directory.ListDirBegin();
		while(true)
		{
			var file = directory.GetNext();
			if(file == "")
			{
				break;
			}
			if(file.Extension() == StudySet.fileextension.Extension())
			{
				StudysetsInFolder.Add(file.Split('.')[0]);
			}
		}
		directory.ListDirEnd();

		StudysetsInFolder.Sort();


		for (int i = 0; i < StudysetsInFolder.Count ; i++)
		{
			StudysetsBrowserButton label = new StudysetsBrowserButton();
			label.Text = StudysetsInFolder[i];
			studysetsContainer.AddChild(label);
		}
	}

	public void LoadStudyset(StudySet studySet)
	{
	}
}
