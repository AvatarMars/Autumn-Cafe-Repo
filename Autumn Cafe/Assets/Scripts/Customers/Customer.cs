using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Timer))]
public class Customer : MonoBehaviour
{
    // TODO: Implement patience system
    [SerializeField] private string _name = "Bill";
    [SerializeField] private MealType _desiredMeal;

    private NavMeshAgent _agent;
    private Timer _timer;

    private Chair _currentChair;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _timer = GetComponent<Timer>();
        _timer.onTimerTickFinished += OnPatienceDepleted;
        _timer.StartTimer();
    }

    public void CheckForFreeChairs()
    {
        if (_currentChair != null) return;

        var chair = ChairManager.Instance.GetFreeChair();

        if (chair != null)
        {
            StartCoroutine(MoveToChair(chair));
        }
    }

    private void OnPatienceDepleted()
    {
        StartCoroutine(MoveToExit());
    }

    IEnumerator MoveToChair(Chair chair)
    {
        _currentChair = chair;
        _agent.SetDestination(_currentChair.transform.position);
        _agent.isStopped = false;

        while (_agent.remainingDistance > .1f)
        {
            yield return null;
        }

        _desiredMeal = MealManager.Instance.GetRandomMeal();
        _timer.StartTimer();
    }

    IEnumerator MoveToExit()
    {
        _agent.SetDestination(CustomerSpawner.Instance.ExitPoint.position);
        _agent.isStopped = false;
        while (_agent.remainingDistance > 1f)
        {
            yield return null;
        }
        Debug.Log($"{_name} got tired of waiting and went home");
        Destroy(gameObject);
    }
}
