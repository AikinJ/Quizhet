using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IScene
{
    SceneManager.Scene thisScene {get;}

    void LoadStudyset(StudySet studySet);
}

