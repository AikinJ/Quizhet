using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StudySet
{
	public static string fileextension = ".qzht";



	private string _name;
	public string name
	{
		get
		{
			if(_name == null)
			{
				_name = "NewStudySet";
			}
			return _name;
		}
		set
		{
			DeleteOldFile();
			_name = value;
			Save();
		}
	}
	public string description;

	public List<Flashcard> flashcards;
	public (Language, Language) frontLanguagebackLanguage;
	/// <summary>
	/// Studyset from importstring
	/// </summary>
	/// <param name="importstring"></param>
	/// <param name="cardseperator"></param>
	/// <param name="sideseperator"></param>
	public StudySet(string importstring, char cardseperator, char sideseperator)
	{
		_name = "";
		description = "";
		flashcards = FlashcardsFromString(importstring, cardseperator, sideseperator);
	}
	

	/// <summary>
	/// Studyset from savestring
	/// </summary>
	/// <param name="savestring"></param>
	public StudySet(string savestring)
	{
		string[] strings = savestring.Split('\n');
		if (strings[0]==Quizhet.version)
		{
			_name =strings[1];
			description = strings[2];
			flashcards = new List<Flashcard>();
			for(int i = 3; i < strings.Length; i++)
			{
				Flashcard flashcard = new Flashcard(strings[i]);
				if (flashcard.frontSide == null) continue;
				flashcards.Add(flashcard);
			}
		}
		else
		{
			//convert from older version
		}
	}

	/// <summary>
	/// New empty studyset
	/// </summary>
	public StudySet()
	{
		_name = "NewStudyset";
		description = "";
		flashcards = new List<Flashcard>();
	}

	public Flashcard AddEmptyFlashcard()
	{
		flashcards.Add(new Flashcard("", "", flashcards.Count));
		Save();
		return flashcards[flashcards.Count - 1];
	}
	public static StudySet LoadStudysetFromFile(string path)
	{
		File file = new File();
		file.Open(path, File.ModeFlags.Read);
		return new StudySet(file.GetAsText());
	}
	public void RemoveFlashcard(int index)
	{
		GD.Print(index);
		flashcards.RemoveAt(index);
		ResetIndexFrom(index);
		Save();
	}
	public void Save()
	{
		File file = new File();
		file.Open(StudysetPath(name), File.ModeFlags.Write);
		GD.Print(StudysetPath(name));
		file.StoreString(saveString);
		file.Close();
	}
	public void ResetIndexFrom(int index)
	{
		for (int i = index; i < flashcards.Count; i++)
		{
			flashcards[i].index = i;
		}
	}

	private static List<Flashcard> FlashcardsFromString(string cardsstring, char cardseperator, char sideseperator)
	{
		List<Flashcard> flashcardlist = new List<Flashcard>();
		string[] cards = cardsstring.Split(cardseperator);
		for (int i = 0; i < cards.Length; i++)
		{
			var flashcard = FlashcardFromString(cards[i], sideseperator);
			if (flashcard == null) continue;
			flashcard.index = flashcardlist.Count;
			flashcardlist.Add(flashcard);
		}
		return flashcardlist;
	}
	private static Flashcard FlashcardFromString(string cardstring, char sidesseperator)
	{
		string[] sides = cardstring.Split(sidesseperator);
		string frontside = sides[0];
		string backside = "";
		if (sides.Length > 1)
		{
			for (int j = 1; j < sides.Length; j++)
			{
				backside += sides[j];
			}
		}
		try{
		return new Flashcard(sides[0], sides[1], 0);
		}
		catch{
			return null;
		}
	}
	private static string StudysetPath(string name)
	{
		return Prefs.currentStudysetsPath + "/" + name + fileextension;
	}

	private string saveString
	{
		get
		{
			string savestring = "";
			savestring += Quizhet.version;
			savestring += '\n';
			savestring += name;
			savestring += '\n';
			savestring += description;
			savestring += '\n';
			for (int i = 0; i < flashcards.Count; i++)
			{
				savestring += flashcards[i].saveString;
				savestring += '\n';
			}
			return savestring;
		}	
	}
	private void DeleteOldFile()
	{
		var dir = new Directory();
		dir.Remove(StudysetPath(name));

	}




}

