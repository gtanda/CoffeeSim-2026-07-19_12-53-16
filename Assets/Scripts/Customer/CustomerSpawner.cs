using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform destinationPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        GameObject customerObject = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        Customer customer = customerObject.GetComponent<Customer>();
        customer.MoveTo(destinationPoint.position);
    }
}