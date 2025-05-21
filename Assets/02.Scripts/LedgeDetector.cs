using System;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;

    public Vector3 ledgePoint { get; private set; }
    public Vector3 ledgeNormal { get; private set; }
    public bool ledgeAvailable { get; private set; } = false;
    public Transform ledgeTransform { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            ledgePoint = other.ClosestPoint(transform.position);
            ledgeNormal = other.transform.forward;
            ledgeAvailable = true;
            ledgeTransform = other.transform;
           // OnLedgeDetect?.Invoke(ledgePoint, ledgeNormal);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            ledgeAvailable = false;
        }
    }
}
