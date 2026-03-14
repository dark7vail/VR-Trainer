using UnityEngine;

public class AccuracyTracker : MonoBehaviour
{
    public static void RecordShot()
    {
        GameManager.Instance?.RegisterShot();
    }

    public static void RecordHit()
    {
        GameManager.Instance?.RegisterHit();
    }
}

