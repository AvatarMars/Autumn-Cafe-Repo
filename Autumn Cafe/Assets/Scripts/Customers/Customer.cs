using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Timer))]
public class Customer : MonoBehaviour
{
    [SerializeField] private string _name = "Bill";
    [SerializeField] private MealType _desiredMeal;
    [SerializeField] private Vector2 _desiredMealSelectionTime;

    public DateTime StoreArrivalTime { get; set; }

    private NavMeshAgent _agent;
    private Timer _timer;

    private Chair _currentChair;
    
    private bool ShouldMoveTowardsTarget =>
        this != null &&
        (_agent.pathPending ||
        _agent.remainingDistance > .1f);

    public bool IsWaiting =>
        _currentChair == null &&
        _timer.IsRunning;

    public bool CanReceiveMeal => _desiredMeal != MealType.None;

    public string Name => _name;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _timer = GetComponent<Timer>();
        _timer.onTimerTickFinished += OnPatienceDepleted;
    }

    public void CheckForFreeChairs()
    {
        if (_currentChair != null || !ChairManager.Instance.ExistsFreeChairs) return;

        _timer.StopTimer();
        var chair = ChairManager.Instance.GetFreeChair();

        if (chair != null)
        {
            StartCoroutine(MoveToChair(chair));
        }
    }

    private void OnPatienceDepleted()
    {
        MoveToExit();
    }

    public void MoveTowardsWaitingPoint(Vector3 point, Func<Vector3> waitingPointProvider)
    {
        StartCoroutine(MoveTowards(
            point,
            afterMoving: () =>
            {
                StartCoroutine(MoveTowards(
                    waitingPointProvider(),
                    afterMoving: () =>
                    {
                        CustomerManager.Instance.AddCustomerToWaitingQueue(this);
                        _timer.MaxTime *= 2f;
                        StoreArrivalTime = DateTime.Now;
                        _timer.StartTimer();
                    }));
            }));
    }

    IEnumerator MoveTowards(
        Vector3 point,
        Action beforeMoving = null,
        Action onMoving = null,
        Action afterMoving = null)
    {
        beforeMoving?.Invoke();

        _agent.SetDestination(point);
        _agent.isStopped = false;
        while (ShouldMoveTowardsTarget)
        {
            onMoving?.Invoke();
            yield return null;
        }

        afterMoving?.Invoke();
    }

    IEnumerator MoveToChair(Chair chair)
    {
        _timer.StopTimer();

        _currentChair = chair;
        var point = _currentChair.transform.position;

        _agent.SetDestination(point);
        _agent.isStopped = false;

        while (ShouldMoveTowardsTarget)
        {
            yield return null;
        }

        var secondsToWait = Random.Range(_desiredMealSelectionTime.x, _desiredMealSelectionTime.y);
        yield return new WaitForSeconds(secondsToWait);

        _desiredMeal = GetRandomMeal();
        _timer.ResetMaxTime();
        _timer.StartTimer();
    }

    void MoveToExit()
    {
        StartCoroutine(MoveTowards(
            CustomerManager.Instance.ExitPoint.position,
            beforeMoving: () =>
            {
                if (_currentChair != null) _currentChair.IsOccupied = false;
                _timer.StopTimer();
            },
            afterMoving: () =>
            {
                CustomerManager.Instance.RemoveFromAllLists(this);
                Destroy(gameObject);
            }));
    }

    private MealType GetRandomMeal()
    {
        var mealTypes = (MealType[])Enum.GetValues(typeof(MealType));

        // Start from 1 to avoid MealType.None
        var index = Random.Range(1, mealTypes.Length);
        var selectedMeal = mealTypes[index];
        Debug.Log($"Selected meal: {selectedMeal}");
        return selectedMeal;
    }

    public bool ReceiveMeal(Meal meal)
    {
        if (meal.mealType == _desiredMeal)
        {
            Debug.Log($"{_name} is satisfied with your services");

            // TODO: Put logic here to check meal quality and activate dialogue

            MoveToExit();
            return true;
        }

        Debug.Log($"{_name} didn't asked for {meal.mealType}");
        return false;
    }
}
