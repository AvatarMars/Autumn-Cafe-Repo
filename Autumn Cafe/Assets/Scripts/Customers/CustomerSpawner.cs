using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [SerializeField] private List<Customer> _availableCustomers;
    [SerializeField] private Vector3 _customersSeparationOffset;
    [SerializeField] private Vector2 _customersSpawnTimeLimits;
    [SerializeField] private int _maxCustomersInQueue = 3;

    private Queue<Customer> _waitingCustomers;
    private bool _shouldInstantiate;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _waitingCustomers = new Queue<Customer>();
    }

    public void InstantiateCustomer()
    {
        if (_waitingCustomers.Count >= _maxCustomersInQueue) return;

        var index = Random.Range(0, _availableCustomers.Count);
        var selectedCustomer = _availableCustomers[index];

        var position = transform.position;
        var instancePosition = new Vector3(
            position.x,
            position.y + selectedCustomer.transform.localScale.y,
            position.z);

        if (_waitingCustomers.Count > 0)
        {
            instancePosition += _customersSeparationOffset * _waitingCustomers.Count;
        }

        var instantiatedCustomer = Instantiate(selectedCustomer.gameObject, instancePosition, Quaternion.identity)
                    .GetComponent<Customer>();

        _waitingCustomers.Enqueue(instantiatedCustomer);
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
            customer.CheckForFreeChairs();
        }
    }

    private void Update()
    {
        AssignChairToCustomer();
    }
}
