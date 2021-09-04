using System;
using UnityEngine;

public class GameStateInputReader : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ManageState();
        }
    }

    private void ManageState()
    {
        switch (_gameManager.State)
        {
            case GameStateType.Playing:
                _gameManager.PauseGame();
                break;
            case GameStateType.Paused:
                _gameManager.ResumeGame();
                break;
        }
    }
}
