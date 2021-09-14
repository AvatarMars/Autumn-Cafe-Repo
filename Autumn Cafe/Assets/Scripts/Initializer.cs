using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Use this class to initialize all objects that are important for the application, such as Managers
/// </summary>
[DefaultExecutionOrder(-100)]
public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        // Add here any other important object
        var gameManager = TryAddingObject<GameManager>(nameof(GameManager));

        var activeSceneName = SceneManager.GetActiveScene().name;
     
        if (activeSceneName.Contains("Level") || activeSceneName == "SampleScene" || activeSceneName.ToLower().StartsWith("dev"))
        {
            // TODO: use Level1 for the scene name instead
            gameManager.SetInitialState(GameStateType.Playing);
        }

        if (activeSceneName == "MainMenu")
        {
            gameManager.SetInitialState(GameStateType.MainMenu);
        }
    }

    private TObject TryAddingObject<TObject>(string objectName)
        where TObject : Object
    {
        var obj = FindObjectOfType<TObject>();
        if (obj == null)
        {
            obj = new GameObject(objectName, typeof(TObject))
                .GetComponent<TObject>();
        }

        return obj;
    }
}
