using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructionZone : MonoBehaviour
{
    public event Action OnDied;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SC_FPSController>())
        {
            OnDied?.Invoke();
        }
    }
}
