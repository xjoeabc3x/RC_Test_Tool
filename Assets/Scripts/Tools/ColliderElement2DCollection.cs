using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderElement2DCollection : MonoBehaviour {
    public ColliderElement2D[] elements;
    bool mAwaked = false;
    private void Awake()
    {
        if (mAwaked)
            return;
        if (ColdMath.IsArrayNullOrEmpty(elements))
        {
            elements = gameObject.GetComponentsInChildren<ColliderElement2D>(true);
        }
        mAwaked = true;
    }
    public virtual void Reset()
    {
        if (!mAwaked)
            Awake();
        if (!ColdMath.IsArrayNullOrEmpty(elements))
        {
            for (int i = 0; i < elements.Length; i++)
                elements[i].Reset();
        }
    }
}
