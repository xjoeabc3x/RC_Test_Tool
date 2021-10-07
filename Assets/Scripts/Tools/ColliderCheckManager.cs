using UnityEngine;
using UnityEngine.Events;

public class ColliderCheckManager : MonoBehaviour {

    [SerializeField] ColliderElementChecker[] checkers;
    [SerializeField] UnityEvent onComplete;
    private void Awake()
    {
        if(ColdMath.IsArrayNullOrEmpty<ColliderElementChecker>(checkers)) {
            checkers = gameObject.GetComponentsInChildren<ColliderElementChecker>(true);
        }
        for (int i = 0; i < checkers.Length; i++) {
            checkers[i].manager = this;
        }
    }
    public void CheckerComplete() {
        // 收到checker完成通知，要檢查是不是全部的checker都完成了
        if (!ColdMath.IsArrayNullOrEmpty<ColliderElementChecker>(checkers)) {
            for (int i = 0; i < checkers.Length; i++) {
                if (!checkers[i].complete)
                    return;
            }
            // 通知全部檢查完成
            onComplete.Invoke();
        }
    }
    public void Reset()
    {
        if (!ColdMath.IsArrayNullOrEmpty<ColliderElementChecker>(checkers)) {
            for (int i = 0; i < checkers.Length; i++)
                checkers[i].Reset();
        }
    }

}
