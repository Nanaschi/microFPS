using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{

    [SerializeField] private DestructionZone destructionZone;
    [SerializeField] private FinishZone _finishZone;
    [SerializeField] private Image _gameOverSign;
    [SerializeField] private Image _victorySign;
    [SerializeField] private float _secondsToWaitLevelToReload;

    private void OnEnable()
    {
        destructionZone.OnDied += DeathReloadLogic;
        _finishZone.OnFinished += FinishReloadLogic;
    }

    private async void FinishReloadLogic()
    {
        _victorySign.gameObject.SetActive(true);

        await Task.Delay(TimeSpan.FromSeconds(_secondsToWaitLevelToReload));
        
        ReloadCurrentLevel();
    }

    private async void DeathReloadLogic()
    {
        _gameOverSign.gameObject.SetActive(true);

        await Task.Delay(TimeSpan.FromSeconds(_secondsToWaitLevelToReload));
        
        ReloadCurrentLevel();
        
        
    }

    private static void ReloadCurrentLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
