using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager Instance;

    public Action onChairEmptyDetected;

    [SerializeField] private List<Chair> _availableChairs;
    private List<Chair> _freeChairs;

    public bool ExistsFreeChairs => _freeChairs.Count > 0;
    public int FreeChairCount => _freeChairs.Count;

    private void Awake()
    {
        Instance = this;
        UpdateFreeChairs();
    }

    private void Update()
    {
        UpdateFreeChairs();
        if (ExistsFreeChairs) onChairEmptyDetected?.Invoke();
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

    public void UpdateFreeChairs() =>
        _freeChairs = _availableChairs.Where(chair => !chair.IsOccupied).ToList();
}
