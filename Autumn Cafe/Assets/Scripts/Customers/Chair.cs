using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] private bool isOccupied;

    
    public bool IsOccupied
    {
        get => isOccupied;
        set
        {
            ChairManager.Instance.UpdateFreeChairs();
            isOccupied = value;
        }
    }
}
