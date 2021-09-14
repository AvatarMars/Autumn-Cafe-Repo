using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Tooltip("Possible spawn positions for Customers, each children of this gameobject will be a Spawn Point")]
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private List<Customer> _availableCustomers;
    [SerializeField] private Vector3 _customersSeparationOffset;
    [SerializeField] private Vector2 _customersSpawnTimeLimits;
    [SerializeField] private int _maxCustomersInQueue = 3;
    [SerializeField] private int _maxCustomersSpawned = 3;

    private Queue<Customer> _waitingCustomers;
    private List<Customer> _seatedCustomers;
    private List<Customer> _spawnedCustomers;
    private Dictionary<Vector3, Customer> _spacesInQueue;
    private Transform _exitPoint;
    private Transform _entrancePoint;
    private int _maxCustomersInWorld;
    private int _spawnedCustomersCount;
    private bool _shouldInstantiate;

    public Transform ExitPoint => _exitPoint;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _waitingCustomers = new Queue<Customer>();
        _seatedCustomers = new List<Customer>();
        _spawnedCustomers = new List<Customer>();
        _spacesInQueue = new Dictionary<Vector3, Customer>();
        _maxCustomersInWorld = _maxCustomersInQueue + ChairManager.Instance.FreeChairCount;

        _exitPoint = transform.Find("ExitPoint");
        _entrancePoint = transform.Find("EntrancePoint");
        _spawnPoints = GameObject.FindGameObjectsWithTag("CustomerSpawnPoint")
            .Select(obj => obj.transform)
            .ToArray();

        ChairManager.Instance.onChairEmptyDetected += AssignChairToCustomer;
        StartSpawning();
    }

    private void Update()
    {
        UpdateWaitingCustomers();

        if (_spawnedCustomersCount >= _maxCustomersSpawned)
        {
            _shouldInstantiate = false;
        }
    }

    public void InstantiateCustomer()
    {
        if (!_shouldInstantiate || _spawnedCustomers.Count >= _maxCustomersInWorld) return;

        var customerIndex = Random.Range(0, _availableCustomers.Count);
        var selectedCustomer = _availableCustomers[customerIndex];

        var spawnPointIndex = Random.Range(0, _spawnPoints.Length);
        var position = _spawnPoints[spawnPointIndex].position;
        var instancePosition = new Vector3(
            position.x,
            position.y + selectedCustomer.transform.localScale.y,
            position.z);

        var instantiatedCustomer = Instantiate(selectedCustomer.gameObject, instancePosition, Quaternion.identity)
                    .GetComponent<Customer>();

        instantiatedCustomer.MoveTowardsWaitingPoint(
            _entrancePoint.position, 
            () => GetFreeSpaceInQueue(instantiatedCustomer));

        _spawnedCustomers.Add(instantiatedCustomer);
        if (!ChairManager.Instance.ExistsFreeChairs) _waitingCustomers.Enqueue(instantiatedCustomer);

        _spawnedCustomersCount++;
    }

    public void StartSpawning()
    {
        _shouldInstantiate = true;
        StartCoroutine(InstantiateRepeating());
    }

    public void StopSpawning()
    {
        _shouldInstantiate = false;
    }

    IEnumerator InstantiateRepeating()
    {
        while (_shouldInstantiate)
        {
            InstantiateCustomer();
            var seconds = Random.Range(_customersSpawnTimeLimits.x, _customersSpawnTimeLimits.y);
            yield return new WaitForSeconds(seconds);
        }
    }

    public void AssignChairToCustomer()
    {
        if (_waitingCustomers.Count > 0 && ChairManager.Instance.ExistsFreeChairs)
        {
            var customer = _waitingCustomers.Dequeue();
            if (customer == null) return;

            customer.CheckForFreeChairs();
            _seatedCustomers.Add(customer);
        }
    }

    public void AddCustomerToWaitingQueue(Customer customer)
    {
        _waitingCustomers.Enqueue(customer);
    }

    public void RemoveFromSeatedCustomers(Customer customer)
    {
        if (_seatedCustomers.Contains(customer)) _seatedCustomers.Remove(customer);
    }

    public void RemoveFromAllLists(Customer customer)
    {
        RemoveFromSeatedCustomers(customer);
        if (_spawnedCustomers.Contains(customer)) _spawnedCustomers.Remove(customer);
        if (_waitingCustomers.Contains(customer)) UpdateWaitingCustomers();

        if (_spacesInQueue.ContainsValue(customer))
        {
            var key = _spacesInQueue.First(pair => pair.Value == customer).Key;
            _spacesInQueue.Remove(key);
        }
    }

    private Vector3 GetFreeSpaceInQueue(Customer customer)
    {
        var counter = 0;
        var pos = _entrancePoint.position + (_customersSeparationOffset * counter) + _customersSeparationOffset;

        while (_spacesInQueue.ContainsKey(pos))
        {
            pos = _entrancePoint.position + (_customersSeparationOffset * ++counter) + _customersSeparationOffset;
        }

        _spacesInQueue.Add(pos, customer);
        return pos;
    }

    private void UpdateWaitingCustomers()
    {
        _waitingCustomers = new Queue<Customer>(
            _spawnedCustomers
                .Where(cust => cust.IsWaitingInQueue)
                .OrderBy(cust => cust.StoreArrivalTime));
    }
}
