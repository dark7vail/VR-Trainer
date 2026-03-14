using UnityEngine;

public class ProVirtualStock : MonoBehaviour
{
    public Transform frontHand;
    public Transform rearHand;
    public Transform stockPoint;

    public float smoothing = 12f;
    public float shoulderStrength = 0.7f;

    private Transform shoulder;
    private bool stockActive;

    void Update()
    {
        if (frontHand == null || rearHand == null)
            return;

        // Base rotation from two hands
        Vector3 direction = frontHand.position - rearHand.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Shoulder stabilization
        if (stockActive && shoulder != null)
        {
            Vector3 shoulderDir = frontHand.position - shoulder.position;
            Quaternion shoulderRotation = Quaternion.LookRotation(shoulderDir);

            targetRotation = Quaternion.Slerp(
                targetRotation,
                shoulderRotation,
                shoulderStrength
            );
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothing
        );
    }

    public void ActivateStock(Transform shoulderTransform)
    {
        shoulder = shoulderTransform;
        stockActive = true;
    }

    public void DeactivateStock()
    {
        stockActive = false;
        shoulder = null;
    }
}