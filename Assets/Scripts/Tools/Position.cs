using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace Setting
{
    [AddComponentMenu("Cold/Setting/PositionViaTransform")]
    public class Position : BaseT<Transform>
    {
        [FormerlySerializedAs("target")]
        [SerializeField] Transform m_Dest;
        [SerializeField, Header("[作用的目標]")]
        Transform m_EffectTarget;
        [SerializeField] bool m_UseOrgPositionForDest;
        [SerializeField] bool mEffectX;
        public bool effectX { set { mEffectX = value; } get { return mEffectX; } }
        [SerializeField] bool mEffectY;
        public bool effectY { set { mEffectY = value; } get { return mEffectY;}}
        [SerializeField] bool mEffectZ;
        public bool effectZ { set { mEffectZ = value; } get { return mEffectZ;}}
        [SerializeField, Header("[世界座標]")]
        bool world = true;
        Vector3 orgPos;
        Transform orgParent;
        protected override void OnAwake()
        {
            if (m_EffectTarget == null)
                m_EffectTarget = transform;
            if (world)
                orgPos = m_EffectTarget.position;
            else
                orgPos = m_EffectTarget.localPosition;
            orgParent = m_EffectTarget.parent;
        }
        protected override void OnReset()
        {
            if (world)
                m_EffectTarget.position = orgPos;
            else
                m_EffectTarget.localPosition = orgPos;

            m_EffectTarget.SetParent(orgParent);
        }

        protected override void Exec(Transform t)
        {
            Vector3 tmp = Vector3.zero;
            Vector3 newPos = Vector3.zero;
            if(world)
            {
                tmp = m_EffectTarget.position;
                newPos = t.position;
            }
            else 
            {
                tmp = m_EffectTarget.localPosition;
                newPos = t.localPosition;
            }
            if (mEffectX) tmp.x = newPos.x;
            if (mEffectY) tmp.y = newPos.y;
            if (mEffectZ) tmp.z = newPos.z;
            m_EffectTarget.SetParent(t);
            if (world)
                m_EffectTarget.position = tmp;
            else
                m_EffectTarget.localPosition = tmp;
        }
        protected override void Exec()
        {
            if(m_UseOrgPositionForDest)
            {
                OnReset();
                return;
            }
            if (m_Dest != null)
                Exec(m_Dest);
        }
        public void ResetOrgPos()
        {
            OnAwake();
        }
        public void ResetOrgParent()
        {
            m_EffectTarget.SetParent(orgParent);
        }
    }
}

