using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace Setting
{
    abstract public class Base : MonoBehaviour
    {
        protected bool mAwaked;
        public int group = 0;
        public string[] actionKeys;
        [FormerlySerializedAs("behaviour"), Header("[改用 actionKeys]")]
        public string actionKey = "";
        [SerializeField] protected bool setWithoutEnabeld = false;
        [SerializeField] protected bool disableWhenPlayed = false;

        abstract protected void Exec();
        public void Set()
        {
            if (!enabled && !setWithoutEnabeld)
                return;
            if (!mAwaked)
                Awake();
            Exec();
            if (disableWhenPlayed)
                enabled = false;
        }
        abstract protected void OnReset();
        public void Reset()
        {
            if (!Application.isPlaying)
                return;
            if (!enabled)
                return;
            if (!mAwaked)
                Awake();
            OnReset();
        }
        protected virtual void OnAwake() { }
        protected void Awake()
        {
            if (mAwaked)
                return;
            mAwaked = true;
            OnAwake();
        }
        public IEnumerable<string> GetActionKeys()
        {
            if (!string.IsNullOrEmpty(actionKey))
                yield return actionKey;
            if (actionKeys == null || actionKeys.Length == 0)
                yield break;
            for (int i = 0; i < actionKeys.Length; i++)
                yield return actionKeys[i];
        }
    }
}

