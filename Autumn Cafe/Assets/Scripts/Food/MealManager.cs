using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MealManager : MonoBehaviour
{
    public static MealManager Instance;

    [SerializeField] private Meal[] _availableMeals;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //_availableMeals = Resources.LoadAll<Meal>("ScriptableObjects/Meals");
    }

    //public Meal GetRandomMeal()
    //{
    //    var index = Random.Range(0, _availableMeals.Length);
    //    return _availableMeals[index];
    //}

    public MealType GetRandomMeal()
    {
        var mealTypes = (MealType[])Enum.GetValues(typeof(MealType));
        
        // Start from 1 to avoid MealType.None
        var index = Random.Range(1, mealTypes.Length);
        return mealTypes[index];
    }
}
