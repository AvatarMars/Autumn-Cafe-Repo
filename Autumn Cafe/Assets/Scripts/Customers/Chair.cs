using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] private bool isOccupied;
    [SerializeField] private Transform mealSpace;

    private GameObject _mealInTable;

    public bool IsOccupied
    {
        get => isOccupied;
        set
        {
            //ChairManager.Instance.UpdateFreeChairs();
            isOccupied = value;
        }
    }

    public void LocateInMealSpace(GameObject obj)
    {
        _mealInTable = obj;
        obj.transform.position = mealSpace.position;
    }

    public void CleanPlace()
    {
        if (!_mealInTable) return;

        Destroy(_mealInTable);
        _mealInTable = null;
    }
}
