using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Test
{
    private static readonly int cardsPerRound = 5;
    private int currentIndex;
    private int currentRound;
    private int currentCardIntoRound;
    private List<Flashcard> flashcardsLeft;

    StudySet studySetForTest;
    public Test(StudySet studyTest)
    {
        this.studySetForTest = studyTest;
        currentIndex = 0;
        currentRound = 0;
        currentCardIntoRound = 0;
        flashcardsLeft = new List<Flashcard>(studySetForTest.flashcards);
    }

    public Flashcard GetNextFlashcard()
    {
        if(currentCardIntoRound >= cardsPerRound || currentIndex >= flashcardsLeft.Count) NextRound();
        Flashcard flashcard = flashcardsLeft[currentIndex];
        currentIndex++;
        currentCardIntoRound++;
        return flashcard;
    }

    public Action OnNextRound;
    private void NextRound()
    {
        currentRound++;
        currentIndex = 0;
        currentCardIntoRound = 0;
        if (OnNextRound != null) OnNextRound();
    }

    public Action OnTestComplete;
    private void TestComplete()
    {
        if(OnTestComplete != null) OnTestComplete();
    }

    public void Answered(bool correct)
    {
        if (correct) 
        {
            currentIndex--;
            flashcardsLeft.RemoveAt(currentIndex);
        }
        if (flashcardsLeft.Count <= 0) TestComplete();
    }
}

