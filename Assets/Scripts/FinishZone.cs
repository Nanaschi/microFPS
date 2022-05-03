using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public event Action OnFinished;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SC_FPSController>())
        {
            OnFinished?.Invoke();
        }
    }
}
