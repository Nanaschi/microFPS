using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{

    [SerializeField] private DestructionZoneController _destructionZoneController;
    [SerializeField] private Image _gameOverSign;
    [SerializeField] private float _secondsToWaitGameOver;

    private void OnEnable()
    {
        _destructionZoneController.OnDied += LevelReloadLogic;
    }

    private async void LevelReloadLogic()
    {
        _gameOverSign.gameObject.SetActive(true);

        await Task.Delay(TimeSpan.FromSeconds(_secondsToWaitGameOver));
        
        UnityEngine.SceneManagement.SceneManager.LoadScene
        (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        
        
    }
}
