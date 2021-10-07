using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ThreeDDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [Header("[ 固定x ]")]
    [SerializeField] bool freezeX;
    [Header("[ 固定y ]")]
    [SerializeField] bool freezeY;
    [Header("[ 固定z ]")]
    [SerializeField] bool freezeZ;
    [SerializeField, Header("[盲區]")]
    Vector3 deadZone = Vector3.zero;
    [SerializeField]
    bool m_AlwaysSendUpEvent;
    [SerializeField, Header("[點擊時移到最前面]")]
    bool m_LastSiblingWhenPressDown;
    Vector3 posDown = new Vector3();
    Vector3 posDownSelf = new Vector3();
    Vector3 posOrg = new Vector3();
    Vector3 delta = Vector3.zero;
    public bool fixedX { set { freezeX = value; } get { return freezeX; } }
    public bool fixedY { set { freezeY = value; } get { return freezeY; } }
    public bool fixedZ { set { freezeZ = value; } get { return freezeZ; } }
    public UnityEventPointerEventData onUnityPointerDown;
    public UnityEventPointerEventData onUnityPointerUp;
    public UnityEventPointerEventData onUnityDrag;
    public UnityEventPointerEventData onUnityEndDrag;
    public UnityEvent onSuccessDrag;
    bool mAwaked;
    bool m_Dragging = false;
    int m_PointerId = -1;
    void Awake() 
    {
        posOrg = transform.position;
        mAwaked = true;
    }
    public void ResetPosition() 
    {
        if(mAwaked)
            transform.position = posOrg;
    }
    public void SetPosition(Vector3 pos) 
    {
        transform.position = PosFreeze(pos);
    }
    public void SetPosition(Transform tPos) 
    {
        transform.position = PosFreeze(tPos.position);
    }
    bool IsNewOrSamePointerId(int pointerId)
    {
        return m_PointerId == -1 || pointerId == m_PointerId;
    }
    #region -- Event --
	public void OnPointerDown(PointerEventData eventData) 
    {
        if (!mAwaked) 
            return;
        if (m_PointerId != -1)
            return;
        m_PointerId = eventData.pointerId;
        delta = Vector3.zero;
        Camera c = eventData.pressEventCamera;
        posDown = PosFreeze(c.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, c.nearClipPlane)));
        posDownSelf = transform.position;
        if (onUnityPointerDown != null)
            onUnityPointerDown.Invoke(eventData);
        m_Dragging = false;
        if (m_LastSiblingWhenPressDown)
            transform.SetAsLastSibling();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_Dragging && !m_AlwaysSendUpEvent)
            return;
        if (!IsNewOrSamePointerId(eventData.pointerId))
            return;
        if (!m_Dragging)
            m_PointerId = -1;
        if (onUnityPointerUp != null)
            onUnityPointerUp.Invoke(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsNewOrSamePointerId(eventData.pointerId))
            return;
        m_Dragging = true;
        Camera c = eventData.pressEventCamera;
        posDown = PosFreeze(c.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, c.nearClipPlane)));
        posDownSelf = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!mAwaked) 
            return;
        if (!IsNewOrSamePointerId(eventData.pointerId))
            return;
        delta.x += eventData.delta.x;
        delta.y += eventData.delta.y;
        SetPosition(eventData);
        if (onUnityDrag != null)
            onUnityDrag.Invoke(eventData);
    }
    public void OnEndDrag(PointerEventData eventData) 
    {
        if (!mAwaked) 
            return;
        if (!IsNewOrSamePointerId(eventData.pointerId))
            return;
        SetPosition(eventData);
        if (onUnityEndDrag != null)
            onUnityEndDrag.Invoke(eventData);
        if (Mathf.Abs(delta.x) >= deadZone.x && Mathf.Abs(delta.y) >= deadZone.y)
        {
            if (onSuccessDrag != null)
                onSuccessDrag.Invoke();
        }
        m_PointerId = -1;
    }
    #endregion
    void SetPosition(PointerEventData eventData) 
    {
        Camera c = eventData.pressEventCamera;
        Vector3 newPos = PosFreeze(c.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, c.nearClipPlane)));
        transform.position = posDownSelf + (newPos - posDown);
    }
    Vector3 PosFreeze(Vector3 posTarget) 
    {
        if(freezeX)
            posTarget.x = posOrg.x;
        if(freezeY)
            posTarget.y = posOrg.y;
        if(freezeZ)
            posTarget.z = posOrg.z;
        return posTarget;
    }
}
