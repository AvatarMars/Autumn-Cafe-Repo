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
        var index = Random.Range(0, mealTypes.Length);
        return mealTypes[index];
    }
}
