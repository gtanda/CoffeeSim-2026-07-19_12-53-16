using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Spawn Settings")] [SerializeField]
    private GameObject customerPrefab;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] Transform coffeePickupPoint;

    [SerializeField] private QueueManager queueManager;

    public int maxCustomers = 5;
    private int currentCustomers; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            StartCoroutine(SpawnCustomerRoutine());
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);

        while (currentCustomers < maxCustomers)
        {
            currentCustomers++;
            GameObject customerObject = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
            Customer customer = customerObject.GetComponent<Customer>();
            customer.SetCoffeePickupPoint(coffeePickupPoint);
            if (!queueManager.TryAssignCustomer(customer))
            {
                Destroy(customerObject);
            }
            
            yield return wait;
        }
    }
}