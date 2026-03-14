using UnityEngine;

public class StockTrigger : MonoBehaviour
{
    public ProVirtualStock stock;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shoulder"))
        {
            stock.ActivateStock(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shoulder"))
        {
            stock.DeactivateStock();
        }
    }
}