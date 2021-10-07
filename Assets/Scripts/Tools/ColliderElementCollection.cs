using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderElementCollection : MonoBehaviour {

    public ColliderElement[] elements;
    private void Awake()
    {
        if (ColdMath.IsArrayNullOrEmpty<ColliderElement>(elements))
        {
            elements = gameObject.GetComponentsInChildren<ColliderElement>(true);
        }
    }
    public virtual void Reset()
    {
        if (!ColdMath.IsArrayNullOrEmpty<ColliderElement>(elements))
        {
            for (int i = 0; i < elements.Length; i++)
                elements[i].Reset();
        }
    }
}
