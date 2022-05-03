using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogController : MonoBehaviour
{
    private string path;
    // Start is called before the first frame update
    void Start()
    {
        CreateLog();
    }

    private void CreateLog()
    {
        path = Application.dataPath + "/Log.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Game Log " +
                                    "\n" +
                                    "\n");
        }
    }


    public void GameStarted()
    {
        string content = $"Game started at {System.DateTime.Now} (GMT +3) \n";
        
        
        File.AppendAllText(path, content);
    }
    
    public void GameFinished()
    {
        string content = $"Game finished at {System.DateTime.Now} (GMT +3) \n";
        
        
        File.AppendAllText(path, content);
    }
    
    public void PlayerDied()
    {
        string content = $"Player died at {System.DateTime.Now} (GMT +3) \n";
        
        
        File.AppendAllText(path, content);
    }
    
    
    public void ShotWasMade()
    {
        string content = $"Shot was made at {System.DateTime.Now} (GMT +3) \n";
        
        
        File.AppendAllText(path, content);
    }
    
    public void PlayerWasHit(EnemyBehaviour enemyBehaviour)
    {
        string content = $"Player was hit by {enemyBehaviour} at {System.DateTime.Now} (GMT +3) \n";
        
        
        File.AppendAllText(path, content);
    }
    
}
