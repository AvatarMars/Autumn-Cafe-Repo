using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomerSpawner))]
public class CustomerSpawnerEditor : Editor
{
    private CustomerSpawner _spawner;
    private MealType _mealType;
    private Customer _customer;

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

        _mealType = (MealType)EditorGUILayout.EnumPopup("Meal Type", _mealType);
        _customer = (Customer)EditorGUILayout.ObjectField("Selected Customer", _customer, typeof(Customer), true);

        if (GUILayout.Button("Give Meal To First Customer"))
        {
            var customer = _spawner.GetFirstSeatedCustomerWithDesiredMeal(_mealType);
            if (_customer != null) _customer.GiveMeal(_mealType);
        }
    }
}
