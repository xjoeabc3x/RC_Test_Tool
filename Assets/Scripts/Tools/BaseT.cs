using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Setting
{
    abstract public class BaseT<T> : Base where T : Object
    {
        abstract protected void Exec(T t);
        public void Set(T t)
        {
            if (!mAwaked)
                Awake();
            Exec(t);
        }
    }
}

