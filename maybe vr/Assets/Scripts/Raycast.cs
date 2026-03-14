using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGun : MonoBehaviour
{
    [Header("References")]
    public Transform barrelPoint;
    public Transform attachTransform;
    public Transform secondaryTransform;

    [Header("Gun Settings")]
    public float raycastRadius = 0.1f;
    public float raycastDistance = 100f;
    public float lineVisibleDuration = 0.05f;

    private LineRenderer lineRenderer;
    private float lineTimer;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool triggerWasPressed = false;
    private bool isHeld = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor primaryInteractor = null;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor secondaryInteractor = null;

    void Start()
    {
        if (barrelPoint != null)
        {
            lineRenderer = barrelPoint.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.enabled = false;
            }
            else Debug.LogWarning("RaycastGun: No LineRenderer on BarrelPoint!");
        }
        else Debug.LogWarning("RaycastGun: BarrelPoint not assigned!");

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
        else Debug.LogWarning("RaycastGun: No XRGrabInteractable found!");
    }

    void OnDestroy()
    {
        if (grabInteractable == null) return;
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor = args.interactorObject;

        if (primaryInteractor == null)
        {
            primaryInteractor = interactor;
            Debug.Log($"[RaycastGun] {GetGrabbingHand(interactor)} is PRIMARY — shoots");
        }
        else
        {
            secondaryInteractor = interactor;
            Debug.Log($"[RaycastGun] {GetGrabbingHand(interactor)} is SECONDARY — support only");
        }
    }

    void OnReleased(SelectExitEventArgs args)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor = args.interactorObject;

        if (interactor == primaryInteractor)
        {
            primaryInteractor = null;
            triggerWasPressed = false;
            Debug.Log("[RaycastGun] Primary released");
        }
        else if (interactor == secondaryInteractor)
        {
            secondaryInteractor = null;
            Debug.Log("[RaycastGun] Secondary released");
        }

        if (primaryInteractor == null && secondaryInteractor == null)
        {
            isHeld = false;
            triggerWasPressed = false;

            if (lineRenderer != null) lineRenderer.enabled = false;
            lineTimer = 0;
            Debug.Log("[RaycastGun] Fully released");
        }
    }

    void Update()
    {
        if (lineTimer > 0)
        {
            lineTimer -= Time.deltaTime;
            if (lineTimer <= 0 && lineRenderer != null)
                lineRenderer.enabled = false;
        }

        if (!isHeld || primaryInteractor == null) return;

        XRNode shootingHand = GetGrabbingHand(primaryInteractor);
        InputDevice device = InputDevices.GetDeviceAtXRNode(shootingHand);

        device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed);

        if (triggerPressed && !triggerWasPressed)
            Shoot();

        triggerWasPressed = triggerPressed;
    }

    void Shoot()
    {
        AccuracyTracker.RecordShot();

        if (barrelPoint == null) return;

        Vector3 origin = barrelPoint.position;
        Vector3 direction = barrelPoint.forward;

        if (Physics.SphereCast(origin, raycastRadius, direction, out RaycastHit hit, raycastDistance))
        {
            ShowLine(origin, hit.point);

            MovingTarget movingTarget = hit.collider.GetComponent<MovingTarget>();
            if (movingTarget != null)
            {
                movingTarget.TakeDamage(1);
                return;
            }

            CountdownTarget countdownTarget = hit.collider.GetComponent<CountdownTarget>();
            if (countdownTarget != null)
            {
                countdownTarget.OnShot();
                return;
            }
        }
        else
        {
            ShowLine(origin, origin + direction * raycastDistance);
        }
    }

    void ShowLine(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) return;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineTimer = lineVisibleDuration;
    }

    XRNode GetGrabbingHand(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor)
    {
        GameObject go = (interactor as MonoBehaviour)?.gameObject;
        if (go != null)
        {
            XRController controller = go.GetComponentInParent<XRController>();
            if (controller != null) return controller.controllerNode;

            string n = go.name.ToLower();
            if (n.Contains("left")) return XRNode.LeftHand;
            if (n.Contains("right")) return XRNode.RightHand;
        }
        return XRNode.RightHand;
    }
}