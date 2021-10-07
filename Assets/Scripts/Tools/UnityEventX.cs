using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[System.Serializable]
public class UnityEventPointerEventData : UnityEvent<PointerEventData> { }
[System.Serializable]
public class UnityEventPassPointerEventData : UnityEvent<PointerEventData> { }
[System.Serializable]
public class UnityEventPassGameObject : UnityEvent<GameObject> { }
[System.Serializable]
public class UnityEventPassTransform : UnityEvent<Transform> { }
[System.Serializable]
public class UnityEventPassBool : UnityEvent<bool> {}
[System.Serializable]
public class UnityEventPassInt : UnityEvent<int> { }
[System.Serializable]
public class UnityEventPassFloat : UnityEvent<float> { }
[System.Serializable]
public class UnityEventPassColor32 : UnityEvent<Color32> { }
