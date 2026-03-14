using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Sprite crosshairImage;

    void Start()
    {
        if (crosshairImage == null)
            Debug.LogWarning("Crosshair: No Image assigned!");
    }
}