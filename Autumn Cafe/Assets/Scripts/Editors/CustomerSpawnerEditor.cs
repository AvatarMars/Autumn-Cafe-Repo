using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomerSpawner))]
public class CustomerSpawnerEditor : Editor
{
    private CustomerSpawner _spawner;
    void OnEnable()
    {
        _spawner = (CustomerSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Instantiate Customer"))
        {
            _spawner.InstantiateCustomer();
        }

        if (GUILayout.Button("Start Spawning"))
        {
            _spawner.StartSpawning();
        }

        if (GUILayout.Button("Stop Spawning"))
        {
            _spawner.StopSpawning();
        }
    }
}
