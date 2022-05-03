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
    [SerializeField] private LogController _logController;
    private void OnEnable()
    {
        destructionZone.OnDied += DeathReloadLogic;
        destructionZone.OnDied += _logController.PlayerDied;
        _finishZone.OnFinished += FinishReloadLogic;
        _finishZone.OnFinished += _logController.GameFinished;
        EnemyBehaviour.OnShotPerformed += _logController.ShotWasMade;
        EnemyBehaviour.OnPlayerHit += _logController.PlayerWasHit;
    }

    private void Start()
    {
        _logController.GameStarted();
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


    private void OnDisable()
    {
        destructionZone.OnDied -= DeathReloadLogic;
        destructionZone.OnDied -= _logController.PlayerDied;
        _finishZone.OnFinished -= FinishReloadLogic;
        _finishZone.OnFinished -= _logController.GameFinished;
        EnemyBehaviour.OnShotPerformed -= _logController.ShotWasMade;
        EnemyBehaviour.OnPlayerHit -= _logController.PlayerWasHit;
    }
}
