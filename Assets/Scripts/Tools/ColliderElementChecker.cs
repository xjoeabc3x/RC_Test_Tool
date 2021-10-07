
using UnityEngine;
using UnityEngine.Events;
public class ColliderElementChecker : ColliderElementCollection {
    public ColliderCheckManager manager;
    public bool complete;
    [SerializeField] UnityEvent onComplete;
    public void IntersectsCompare(GameObject go) {
        if (complete) return;
        ColliderElement inputElement = go.GetComponent<ColliderElement>();
        if(inputElement != null) {
            // 比較
            complete = elements.Length > 0;
            for (int i = 0; i < elements.Length; i++) {
                if (elements[i].matchElement != null)
                    continue;
                if(!elements[i].IntersectsCompare(inputElement)) {
                    if(complete)
                        complete = (elements[i].matchElement != null);
                }
            }
            // 如果沒有任何香蕉，通知沒香蕉
            if (inputElement.matchElement == null)
                inputElement.NotIntersectsInvoke(inputElement.transform);
            // 完成後通知管理者
            if(complete) {
                if(manager != null)
                    manager.CheckerComplete();
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
