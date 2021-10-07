using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class UnityEventPassColliderElement2D : UnityEvent<ColliderElement2D> { }
public class ColliderElement2DChecker : ColliderElement2DCollection {
    [SerializeField] UnityEvent onComplete;
    bool complete = false;
    public void IntersectsCompare(ColliderElement2D input)
    {
        if (complete) return;
        if (input != null)
        {
            // 比較
            complete = elements.Length > 0;
            bool pass = false;
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].match != null)
                    continue;
                if (!elements[i].IntersectsCompare(input))
                {
                    if (complete)
                        complete = (elements[i].match != null);
                }
                else
                    pass = true;
            }
            // 如果沒有任何香蕉，通知沒香蕉
            if (!pass)
                input.NotIntersectsInvoke(input.transform);
            // 完成後通知管理者
            if (complete)
            {
                //if (manager != null)
                    //manager.CheckerComplete();
                onComplete.Invoke();
            }
        }
    }
    public override void Reset()
    {
        base.Reset();
        complete = false;
    }
}
