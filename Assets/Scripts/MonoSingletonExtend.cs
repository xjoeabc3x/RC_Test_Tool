using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MonoSingletonExtend<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T mInstance = null;
    public static T Instance {
        get {
            if(mInstance == null) {
                (new GameObject(typeof(T).ToString() + "#")).AddComponent<T>();
            }
            return mInstance;
        }
    }
    [Header("[換場景時不要刪除]")]
    [SerializeField] protected bool dontDestroy = true;
    protected virtual void OnAwake() {}
    void Awake() {
        if (mInstance == null)
        {
            mInstance = this as T;
            if (dontDestroy)
                DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    public void UndoDontDestroyOnLoad()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}
