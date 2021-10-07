using UnityEngine;
using UnityEngine.Events;
public class ColliderElement : MonoBehaviour {

    public string key;
    public Collider target;
    public ColliderElement matchElement;
    [SerializeField] UnityEvent onReset;
    [SerializeField] UnityEventPassTransform onIntersects;
    [SerializeField] UnityEventPassTransform onNotIntersects;

    private void Awake()
    {
        if (target == null)
            target = gameObject.GetComponent<Collider>();
        if (string.IsNullOrEmpty(key))
            key = gameObject.name;
    }

    public bool IntersectsCompare(ColliderElement compare) {
        
        if(compare.matchElement == null && compare.key.Equals(key)) {
            if(compare.target.bounds.Intersects(target.bounds)) {
                compare.matchElement = this;
                matchElement = compare;
                IntersectsInvoke(matchElement.transform);
                compare.IntersectsInvoke(transform);
                return true;
            }
        }
        // 沒香蕉
        NotIntersectsInvoke(compare.transform);
        return false;
    }

    public void Reset() {
        matchElement = null;
        if(onReset != null)
            onReset.Invoke();
    }
    public void IntersectsInvoke(Transform t) {
        // 香蕉通知
        onIntersects.Invoke(t);
    }
    public void NotIntersectsInvoke(Transform t) {
        // 不香蕉通知
        onNotIntersects.Invoke(t);
    }
    public void SetNewKey(string NewKey)
    {
        key = NewKey;
    }
}
