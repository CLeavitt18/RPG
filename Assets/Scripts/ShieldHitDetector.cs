using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitDetector : MonoBehaviour
{
    [SerializeField] private bool hit;
    [SerializeField] public bool Protecting;

    private void OnTriggerEnter(Collider other) 
    {
        hit = true;  
    }

    private void OnTriggerExit(Collider other) 
    {
        hit = false;    
    }

    public bool GetHit()
    {
        return hit;
    }
}
