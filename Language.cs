using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Language
{
    public string Name { get; set; }
    public char[] characters;

    public Language(String name, Char[] characters)
    {
        Name = name;
        this.characters = characters;
    }
}

