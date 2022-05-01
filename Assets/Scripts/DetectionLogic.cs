using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionLogic : MonoBehaviour
{

    public event Action<Collider> OnDetected;
    public event Action OnDetectionLost;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SC_FPSController>())
        {
            OnDetected?.Invoke(other);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SC_FPSController>())
        {
            OnDetectionLost?.Invoke();
        }
    }
}
