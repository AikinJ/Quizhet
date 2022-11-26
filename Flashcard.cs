using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Flashcard
{
    public int index;
    public string frontSide;
    public string backSide;
    public double experience;

    public Flashcard(String frontSide, String backSide, int index)
    {
        this.frontSide = frontSide;
        this.backSide = backSide;
        this.experience = 0;
        this.index = index;
    }

    public Flashcard(string savestring)
    {
        string[] strings = savestring.Split('|');
        if (strings.Length != 4) 
        {
            GD.Print($"Something went wrong converting {savestring} into a flashcard"); 
        }
        else
        {
            index = int.Parse(strings[0]);
            frontSide = strings[1];
            backSide = strings[2];
            experience = double.Parse(strings[3]);
        }
    }

    public override String ToString()
    {
        return $"{frontSide} | {backSide}";
    }

    public string saveString
    {
        get
        {
            return $"{index}|{frontSide}|{backSide}|{experience}";
        }
    }
}


