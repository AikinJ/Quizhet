using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal static class Prefs
{
    /// <summary>
    /// The prefname of the studysetspath pref
    /// </summary>
    public static readonly string studysetsPathPref = "StudysetsPath";
    /// <summary>
    /// The filename of the prefs file
    /// </summary>
    public static readonly string prefsFileName = "prefs.txt";

    /// <summary>
    /// Add a pref to the pref file
    /// </summary>
    /// <param name="name of the pref"></param>
    /// <param name="value of the pref"></param>
    static private void Addpref(string prefname, string prefvalue)
    {
        GD.Print("adding pref");
        File file = new File();
        string originalstring = "";
        if (file.FileExists(prefsFileName))
        {
            GD.Print("pref file exists");
            file.Open(prefsFileName, File.ModeFlags.Read);
            originalstring = file.GetAsText();
            GD.Print(originalstring);
        }
        file.Open(prefsFileName, File.ModeFlags.Write);
        file.StoreString(originalstring);
        file.StoreString($"{prefname}={prefvalue}" + '\n');
        file.Close();
        file.Open(prefsFileName, File.ModeFlags.Read);
        GD.Print($"prefsfile: {file.GetAsText()}");
        GD.Print(file.GetPath());
    }

    /// <summary>
    /// Try to get a pref from the pref file
    /// </summary>
    /// <param name="prefname"></param>
    /// <returns>"prefvalue, was able to get the pref"</returns>
    static public (string, bool) GetPref(string prefname)
    {
        GD.Print("getting pref");
        File file = new File();
        if (file.FileExists(prefsFileName))
        {
            GD.Print("pref file exists");
            file.Open(prefsFileName, File.ModeFlags.Read);
            string[] prefsstrings = file.GetAsText().Split('\n');
            file.Close();
            for (int i = 0; i < prefsstrings.Length; i++)
            {
                GD.Print(prefsstrings[i]);
                string[] sides = prefsstrings[i].Split('=');
                if (sides.Length != 2) continue;
                if (sides[0] == prefname) { return (sides[1], true); };
            }
            GD.Print($"Prefs do not contain pref {prefname}");
            if(prefname == studysetsPathPref)
            {
                Addpref(prefname, OS.GetExecutablePath());
            }
            else Addpref(prefname, "");
            return ($"Prefs do not contain pref {prefname}", false);
        }
        else
        {
            file.Close();
            Addpref(studysetsPathPref, OS.GetExecutablePath());
            GD.Print("pref file does not exist");
            return ($"Prefs file does not exist", false);
        }
    }

    /// <summary>
    /// Try to change a pref in the pref file, if the pref does not exist yet it creates it
    /// </summary>
    /// <param name="prefname"></param>
    /// <param name="prefvalue"></param>
    static public void ChangePref(string prefname, string prefvalue)
    {
        File file = new File();
        if (file.FileExists(prefsFileName))
        {
            file.Open(prefsFileName, File.ModeFlags.Read);
            string[] prefsstrings = file.GetAsText().Split('\n');
            file.Close();
            for (int i = 0; i < prefsstrings.Length; i++)
            {
                string[] sides = prefsstrings[i].Split('=');
                if (sides.Length != 2) continue;
                if (sides[0] == prefname)
                {
                    sides[1] = prefvalue;
                    string line = sides[0] + '=' + sides[1];
                    prefsstrings[i] = line;

                    file.Open(prefsFileName, File.ModeFlags.Write);
                    for (int j = 0; j < prefsstrings.Length; j++)
                    {
                        file.StoreString(prefsstrings[j] + '\n');
                    }
                    return;
                }
            }
        }
        else Addpref(prefname, prefvalue);

        if (prefname == studysetsPathPref) currentStudysetsPath = prefvalue;
    }

    private static string _currentStudysetsPath;
    /// <summary>
    /// The last studysetspath the script knows of. Should always be right
    /// </summary>
    public static string currentStudysetsPath
    {
        get
        {
            if(_currentStudysetsPath == null)
            {
                (string, bool) path = GetPref(studysetsPathPref);
                if (path.Item2) _currentStudysetsPath = path.Item1;
                else _currentStudysetsPath = System.IO.Path.GetDirectoryName(OS.GetExecutablePath());               
            }
            return _currentStudysetsPath;
        }
        private set
        {
            _currentStudysetsPath = value;
        }
    }
}

