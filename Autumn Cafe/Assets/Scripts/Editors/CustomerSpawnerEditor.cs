//using UnityEditor;
//using UnityEngine;

////[CustomEditor(typeof(CustomerManager))]
//public class CustomerSpawnerEditor : Editor
//{
//    private CustomerManager _manager;
//    private MealType _mealType;
//    private Customer _customer;

//    void OnEnable()
//    {
//        _manager = (CustomerManager)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("Instantiate Customer"))
//        {
//            _manager.InstantiateCustomer();
//        }

//        if (GUILayout.Button("Start Spawning"))
//        {
//            _manager.StartSpawning();
//        }

//        if (GUILayout.Button("Stop Spawning"))
//        {
//            _manager.StopSpawning();
//        }

//        //_mealType = (MealType)EditorGUILayout.EnumPopup("Meal Type", _mealType);
//        //_customer = (Customer)EditorGUILayout.ObjectField("Selected Customer", _customer, typeof(Customer), true);

//        //if (GUILayout.Button("Give Meal To First Customer"))
//        //{
//        //    var customer = _manager.GetFirstSeatedCustomerWithDesiredMeal(_mealType);
//        //    if (_customer != null) _customer.GiveMeal(_mealType);
//        //}
//    }
//}
