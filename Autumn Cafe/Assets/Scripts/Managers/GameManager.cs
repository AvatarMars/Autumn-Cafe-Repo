using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    /*
     * TODO:
     *  - Just display at full color active customer when on dialogue
     *  - Make sure game flow works, from dialogue to menu, and viceversa
     *  - Try to define Drag n' Drop prefabs for quick level design iterations
     */
    public static GameManager Instance;

    public Action onPause;
    public Action onResume;
    public Action onDialogueEnter;
    public Action onDialogueExit;

    [field: SerializeField] public GameStateType State { get; private set; }

    private FirstPersonController _fpsController;
    private GameStateType _lastState;

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

    public void EnterDialogueMode()
    {
        onDialogueEnter?.Invoke();
        State = GameStateType.OnDialogue;
        PrepareToEnterMenu();

        // This will pause the entire game, but animations with Update Mode = Unscaled Time will still reproduce
        Time.timeScale = 0;
    }

    public void ExitDialogueMode()
    {
        onDialogueExit?.Invoke();
        State = GameStateType.Playing;
        PrepareToPlay();

        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (State != GameStateType.Playing && State != GameStateType.OnDialogue) return;

        TryFindScripts();

        _lastState = State;

        State = GameStateType.Paused;
        PrepareToEnterMenu();
        onPause?.Invoke();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (State != GameStateType.Paused) return;

        TryFindScripts();

        State = (_lastState == GameStateType.Playing || _lastState == GameStateType.OnDialogue) ? _lastState : GameStateType.Playing;
        if (State != GameStateType.OnDialogue)
        {
            Time.timeScale = 1;
            PrepareToPlay();
        }
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