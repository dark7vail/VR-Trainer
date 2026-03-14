using UnityEngine;

public class VRSightVisibility : MonoBehaviour
{
    public Transform vrCamera;
    public Transform barrelPoint;
    public Renderer sightRenderer;

    public float aimThreshold = 0.95f;

    void Update()
    {
        if (vrCamera == null || barrelPoint == null || sightRenderer == null) return;

        Vector3 cameraForward = vrCamera.forward;
        Vector3 gunForward = barrelPoint.forward;

        float alignment = Vector3.Dot(cameraForward, gunForward);

        if (alignment > aimThreshold)
            sightRenderer.enabled = true;
        else
            sightRenderer.enabled = false;
    }
}