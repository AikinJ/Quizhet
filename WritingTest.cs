using Godot;
using System;
using System.Collections.Generic;

public class WritingTest : VBoxContainer, IScene
{
	private Label questionSide;
	private LineEdit answerSide;
	public static WritingTest writingTest;

	private RichTextLabel correctionLabel;

	Test test;

	private bool _shouldBeReversed = false;
	public bool shouldBeReversed
	{
		get
		{
			return _shouldBeReversed;
		}
		set
		{
			_shouldBeReversed = value;
		}
	}

	public SceneManager.Scene thisScene { get { return SceneManager.Scene.WritingTest; } }

	public override void _Ready()
	{
		StudySet studySet = new StudySet("wat is een bij?#een insectje\nwat is honing?#een zoete substantie gemaakt door de bij\nwat is honing?#een zoete substantie gemaakt door de bij", '\n', '#');


		writingTest = this;
		questionSide = GetNode<Label>("QuestionSide");
		answerSide = GetNode<LineEdit>("AnswerSide");
		correctionLabel = GetNode<RichTextLabel>("RichTextLabel");
		characterButton = GetNode<CharacterButton>("HBoxContainer/Button");
		characterButton.Visible = false;
		answerSide.Connect("text_changed", this, "OnTextChangedInAnswerSide");
		answerSide.Connect("text_entered", this, "OnTextEnteredInAnswerSide");

		questionSide.Text = "";
		answerSide.Text = "";
		correctionLabel.BbcodeEnabled = true;
		correctionLabel.Visible = false;
		LoadStudyset(studySet);
	}

	
	public void LoadStudyset(StudySet studySet)
	{
		test = new Test(studySet);
		test.OnTestComplete += EndOfTest;
		LoadFlashcard(test.GetNextFlashcard());
		CreateCharacterButtons(Database.Polish);

	}

	CharacterButton characterButton;
	private void CreateCharacterButtons(Language language)
	{
		Node parent = characterButton.GetParent();
		foreach(char ch in language.characters)
		{
			CharacterButton newcharacterbutton = (CharacterButton)characterButton.Duplicate();
			parent.AddChild(newcharacterbutton);
			newcharacterbutton.Text = ch.ToString().ToLower();
			newcharacterbutton.writingTest = this;
			newcharacterbutton.Connect("pressed", newcharacterbutton, "pressed");
			newcharacterbutton.Visible = true;
		}
	}

	public void CharacterButtonPressed(char ch)
	{
		if (answerSide.Text == null) answerSide.Text = "";
		answerSide.Text += ch;
		answerSide.GrabFocus();
		answerSide.CaretPosition = answerSide.Text.Length;
	}

	Flashcard currentFlashcard;
	string currentCorrectAnswer;
	public void LoadFlashcard(Flashcard flashcard)
	{
		if (flashcard == null) return;
		currentFlashcard = flashcard;
		if (!shouldBeReversed)
		{
			questionSide.Text = flashcard.frontSide;
			questionSide.Visible = true;
			currentCorrectAnswer = flashcard.backSide;

		}
		else
		{
			questionSide.Text = flashcard.backSide;
			currentCorrectAnswer = flashcard.frontSide;

		}

		state = State.answering;

	}

	private State state;
	private enum State
	{
		answering,
		practicerewriting,
		corrected
	}

	private void Next()
	{
		if (state == State.answering)
		{
			CheckAnswer();
		}
		else if(state == State.corrected)
		{
			correctionLabel.Visible = false;
			LoadFlashcard(test.GetNextFlashcard());
		}
	}

	static readonly string redcolourstart = "[color=#eb4b4b]";
	static readonly string greencolourstart = "[color=#6fbf8a]";
	static readonly string colourend = "[/color]";

	private void CheckAnswer()
	{
		bool isanswercorrect;
		string answer = answerSide.Text;
		if (answer.ToLower() != currentCorrectAnswer.ToLower())
		{
			isanswercorrect = false;
			state = State.practicerewriting;
			string displaytext = $"You said: ";
			string[] answeredwords = answer.Split(' ');
			string[] correctwords = currentCorrectAnswer.Split(' ');

			List<int> incorrectwords = new List<int>();
			for (int i = 0; i < answeredwords.Length; i++)
			{
				if (i >= correctwords.Length)
				{
					displaytext += $"{redcolourstart}{answeredwords[i]}{colourend}";
				}
				else if(answeredwords[i].ToLower() == correctwords[i].ToLower())
				{
					displaytext += answeredwords[i];
					displaytext += ' ';
				}
				else
				{
					incorrectwords.Add(i);
					bool lastwasincorrect = false;
					for (int j = 0; j < answeredwords[i].Length; j++)
					{
						if (j >= correctwords[i].Length)
						{
							GD.Print("answer given word is longer than correct word");
							if (!lastwasincorrect)
							{
								displaytext += redcolourstart;
							}
							displaytext += answeredwords[i][j];
							lastwasincorrect = true;
						}
						else if (correctwords[i][j] == answeredwords[i][j])
						{
							GD.Print($"letter {correctwords[i][j]} matches");
							if (lastwasincorrect)
							{
								lastwasincorrect = false;
								displaytext += colourend;
							}
							displaytext += correctwords[i][j];
						}
						else
						{
							GD.Print($"letter {answeredwords[i][j]} is wrong");
							if (!lastwasincorrect)
							{
								displaytext += redcolourstart;
								lastwasincorrect = true;
							}
							displaytext += answeredwords[i][j];
						}
						if (j == answeredwords[i].Length - 1)
						{
							GD.Print($"this happens");
							if (lastwasincorrect) displaytext += colourend;
							displaytext += ' ';
						}
					}
				}

			}

			displaytext += $"\n\nCorrect answer: ";
			for(int i =0; i < correctwords.Length; i++)
			{
				if (i != 0) displaytext += ' ';
				bool waswrong = false;
				if (incorrectwords.Contains(i))
				{
					displaytext += greencolourstart;
					waswrong = true;
				}
				displaytext += correctwords[i];
				if (waswrong) displaytext += colourend;
			}

			displaytext += "\nWrite the correct answer again:";
			correctionLabel.BbcodeText = displaytext;
		}
		else
		{
			correctionLabel.BbcodeText = "Correct! good job";
			isanswercorrect = true;
			state = State.corrected;
		}
		questionSide.Visible = false;
		correctionLabel.Visible = true;
		answerSide.Text = "";
		test.Answered(isanswercorrect);
	}

	public void OnTextChangedInAnswerSide(string newtext)
	{
		if(state == State.practicerewriting)
		{
			if(newtext.ToLower() == currentCorrectAnswer.ToLower())
			{
				state = State.corrected;
				correctionLabel.BbcodeText = "";
				questionSide.Text = "";
				answerSide.Text = "";
				Next();
			}
		}
	}

	public void OnTextEnteredInAnswerSide(string newtext)
	{
		Next();
	}

	private void EndOfTest()
	{
		answerSide.Visible = false;
		questionSide.Visible = false;
		correctionLabel.Visible = true;
		correctionLabel.Text = "Congratulations! test complete";
	}
}
