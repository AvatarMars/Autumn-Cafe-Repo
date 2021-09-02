using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager Instance;

    [SerializeField] private List<Chair> _availableChairs;
    private List<Chair> _freeChairs;

    public bool ExistsFreeChairs => _freeChairs.Count > 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateFreeChairs();
    }

    public Chair GetFreeChair()
    {
        var chair = _freeChairs.FirstOrDefault(ch => !ch.IsOccupied);
        if (chair != null)
        {
            chair.IsOccupied = true;
            UpdateFreeChairs();
        }

        return chair;
    }

    private void UpdateFreeChairs() =>
        _freeChairs = _availableChairs.Where(chair => !chair.IsOccupied).ToList();
}
