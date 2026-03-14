using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerVisibility : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The mesh/model of the controller to show or hide")]
    public GameObject controllerModel;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor directInteractor;

    void Start()
    {
        directInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();

        if (directInteractor == null)
        {
            Debug.LogWarning("ControllerVisibility: No XRDirectInteractor found!");
            return;
        }

        directInteractor.selectEntered.AddListener(OnGrab);
        directInteractor.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        if (directInteractor != null)
        {
            directInteractor.selectEntered.RemoveListener(OnGrab);
            directInteractor.selectExited.RemoveListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        SetControllerVisible(false);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        SetControllerVisible(true);
    }

    void SetControllerVisible(bool visible)
    {
        if (controllerModel != null)
            controllerModel.SetActive(visible);
    }
}
