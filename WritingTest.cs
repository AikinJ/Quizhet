using Godot;
using System;
using System.Collections.Generic;

public class WritingTest : VBoxContainer, IScene
{
	public static WritingTest writingTest;

	Label questionSide;
	LineEdit answerSide;
	Test test;
	RichTextLabel correctionLabel;
	
	bool _shouldBeReversed = false;

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


	public override void _Ready()
	{
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

	static string redcolor = "eb4b4b";
	static string greencolor = "6fbf8a";
    static readonly string redcolourstart = "[color=#eb4b4b]";
	static readonly string greencolourstart = "[color=#6fbf8a]";
	static readonly string colourend = "[/color]";


	private string MakeStringColor(string stringtocolor, string color)
	{
		return $"[color=#{color}]{stringtocolor}{colourend}";
	}

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
				//the current word exceeds the number of words in correct answer
				if (i >= correctwords.Length)
				{
					displaytext += MakeStringColor(answeredwords[i], redcolor);
                    incorrectwords.Add(i);
				}
				//the current word is the same as the correct word
				else if(answeredwords[i].ToLower() == correctwords[i].ToLower())
				{
					displaytext += answeredwords[i];
					displaytext += ' ';
				}
				//the current word is incorrect
				else
				{
                    incorrectwords.Add(i);		
                    bool lastcharacterwasincorrect = false;
					for (int j = 0; j < answeredwords[i].Length; j++)
					{
						//character exceeds number of characters in correct word
						if (j >= correctwords[i].Length)
						{
							if (!lastcharacterwasincorrect)displaytext += redcolourstart;
							displaytext += answeredwords[i][j];
							lastcharacterwasincorrect = true;
						}
						//character is correct
						else if (correctwords[i][j].ToString().ToLower() == answeredwords[i][j].ToString().ToLower())
						{
							if (lastcharacterwasincorrect)
							{
								lastcharacterwasincorrect = false;
								displaytext += colourend;
							}
							displaytext += correctwords[i][j];
						}
						//character is incorrect
						else
						{
							if (!lastcharacterwasincorrect)
							{
								displaytext += redcolourstart;
								lastcharacterwasincorrect = true;
							}
							displaytext += answeredwords[i][j];
						}
						//last character, needs to add the colorend if the color is open
						if (j == answeredwords[i].Length - 1)
						{
							if (lastcharacterwasincorrect) displaytext += colourend;
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
			CheckPracticeAnswer(newtext);
        }
	}

	private void CheckPracticeAnswer(string newtext)
	{
        if (newtext.ToLower() == currentCorrectAnswer.ToLower())
        {
            state = State.corrected;
            correctionLabel.BbcodeText = "";
            questionSide.Text = "";
            answerSide.Text = "";
            Next();
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
	public SceneManager.Scene thisScene { get { return SceneManager.Scene.WritingTest; } }
}
