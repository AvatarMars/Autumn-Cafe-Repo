using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Action onPause;
    public Action onResume;

    [field: SerializeField] public GameStateType State { get; private set; }

    private FirstPersonController _fpsController;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TryFindScripts();
    }

    public void SetInitialState(GameStateType gameState)
    {
        State = gameState;

        switch (State)
        {
            case GameStateType.Playing:
                PrepareToPlay();
                break;

            default:
                PrepareToEnterMenu();
                break;
        }
    }

    public void PauseGame()
    {
        if (State != GameStateType.Playing) return;

        TryFindScripts();

        State = GameStateType.Paused;
        PrepareToEnterMenu();
        onPause?.Invoke();
    }

    public void ResumeGame()
    {
        if (State != GameStateType.Paused) return;

        TryFindScripts();

        State = GameStateType.Playing;
        PrepareToPlay();
        onResume?.Invoke();
    }

    public void PrepareToPlay()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _fpsController?.Activate();
    }

    public void PrepareToEnterMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _fpsController?.Deactivate();
    }

    private void TryFindScripts()
    {
        if (_fpsController == null) _fpsController = FindObjectOfType<FirstPersonController>();
    }
}