using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateLog();
    }

    private void CreateLog()
    {
        string path = Application.dataPath + "/Log.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Game Log \n\n");
        }

        string content = "Games started: \n" +
                         "Shots performed: \n" +
                         "Hit by shots: \n" +
                         "Games Lost: \n" +
                         "Games Won: \n";
        
        
        File.AppendAllText(path, content);
    }
}
