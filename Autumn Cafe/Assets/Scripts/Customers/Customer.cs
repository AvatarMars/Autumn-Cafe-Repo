using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [SerializeField] private string _name = "Bill";
    [SerializeField] private MealType _desiredMeal;

    private NavMeshAgent _agent;

    private Chair _currentChair;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void CheckForFreeChairs()
    {
        if (_currentChair != null) return;

        var chair = ChairManager.Instance.GetFreeChair();

        if (chair != null)
        {
            MoveTowards(chair);
        }
    }

    private void MoveTowards(Chair chair)
    {
        _currentChair = chair;
        _agent.SetDestination(_currentChair.transform.position);
        //transform.position = _currentChair.transform.position;
        _agent.isStopped = false;

        // TODO: do this logic after some time when the customer reach a chair
        _desiredMeal = MealManager.Instance.GetRandomMeal();
    }

    private void Update()
    {
        // Apply logic when agent is near of chair
    }
}
