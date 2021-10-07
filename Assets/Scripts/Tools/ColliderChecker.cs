using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ColliderChecker : MonoBehaviour {
    [Header("[ A ]")]
    [SerializeField] Collider colliderA;
    [Header("[ B ]")]
    [SerializeField] Collider colliderB;

    [SerializeField] UnityEvent onIntersects;
    [SerializeField] UnityEvent onNotIntersects;

    public void CheckABIntersects() {
        if (colliderA.bounds.Intersects(colliderB.bounds))
        {
            onIntersects.Invoke();
        }
        else 
        {
            onNotIntersects.Invoke();

        }
            
    }	
}
