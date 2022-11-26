using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Database: Node
{
    private static Database instance;

    public override void _Ready()
    {
        instance = this;
    }

    public static readonly Language baselang = new Language("base",new char[0]);
    public static readonly Language Polish = new Language("Polish", new char[] { 'Ą', 'Ć', 'Ę', 'Ł', 'Ń', 'Ó', 'Ś', 'Ź', 'Ż' });
}

