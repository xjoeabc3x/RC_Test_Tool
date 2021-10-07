using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ColliderElement2D : MonoBehaviour {
    public string key;
    public bool passive = true;
    public bool autoClearEnter = true;
    public Collider2D target;
    public ColliderElement2D match;
    [SerializeField] UnityEvent onReset;
    [SerializeField] UnityEventPassTransform onIntersects;
    [SerializeField] UnityEventPassTransform onNotIntersects;
    [SerializeField] List<Collider2D> listEnter = new List<Collider2D>();
    bool defaultPassive;
    bool mAwaked = false;
	// Use this for initialization
	void Awake () {
        if (mAwaked)
            return;
        if (target == null)
            target = gameObject.GetComponentInChildren<Collider2D>(true);
        defaultPassive = passive;
        match = null;
        mAwaked = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!listEnter.Contains(collision))
            listEnter.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        listEnter.Remove(collision);
    }

    public bool IntersectsCompare(ColliderElement2D compare)
    {
        // 被動才會檢查
        if (!passive)
            return false;
        if (compare.match == null && compare.key.Equals(key))
        {
            if(listEnter.Contains(compare.target))
            {
                compare.match = this;
                match = compare;
                IntersectsInvoke(match.transform);
                compare.IntersectsInvoke(transform);
                return true;
            }
        }
        // 沒香蕉
        NotIntersectsInvoke(compare.transform);
        return false;
    }

    public void SetPassive(bool b)
    {
        passive = b;
    }
    public void Reset()
    {
        if (!mAwaked)
            Awake();
        match = null;
        passive = defaultPassive;
        if (autoClearEnter)
            listEnter.Clear();
        if (onReset != null)
            onReset.Invoke();
    }

    public void IntersectsInvoke(Transform t)
    {
        // 香蕉通知
        onIntersects.Invoke(t);
    }
    public void NotIntersectsInvoke(Transform t)
    {
        // 不香蕉通知
        onNotIntersects.Invoke(t);
    }

}
