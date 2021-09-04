using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Use this class to initialize all objects that are important for the application, such as Managers
/// </summary>
public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        // Add here any other important object
        var gameManager = TryAddingObject<GameManager>(nameof(GameManager));

        switch (SceneManager.GetActiveScene().name)
        {
            case "SampleScene": // TODO: use Level1 for the scene name instead
                gameManager.SetInitialState(GameStateType.Playing);
                break;
            case "MainMenu":
                gameManager.SetInitialState(GameStateType.MainMenu);
                break;
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
