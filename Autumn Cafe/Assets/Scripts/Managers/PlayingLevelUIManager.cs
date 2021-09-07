using System;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class PlayingLevelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _optionsUI;
    private GameObject[] _allMenuUi;

    private void Start() => _allMenuUi = new[] { _pauseMenuUI, _optionsUI };

    private void OnEnable()
    {
        GameManager.Instance.onPause += ShowPauseMenu;
        GameManager.Instance.onResume += HideAllMenus;
    }

    public void HideAllMenus()
    {
        foreach (var obj in _allMenuUi)
        {
            obj.SetActive(false);
        }
    }

    public void ResumeGame() => GameManager.Instance.ResumeGame();

    public void ShowOptionsMenu()
    {
        HideAllMenus();
        _optionsUI.SetActive(true);
    }


    public void ShowPauseMenu()
    {
        HideAllMenus();
        _pauseMenuUI.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.onPause -= ShowPauseMenu;
        GameManager.Instance.onResume -= HideAllMenus;
    }
}
