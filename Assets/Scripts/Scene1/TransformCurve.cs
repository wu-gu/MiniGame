using System;
using UnityEngine;

namespace WNEngine
{
    [System.Serializable]
    public class TransformCurve : MonoBehaviour
    {
        delegate float GetElapsedTime();

        float m_StartTime = 0.0f;
        bool m_Running = false;

        public bool m_UseUnscaledTime = false;
        public Transform m_Transform;

        GetElapsedTime m_GetElapsedTime;

        float GetUnscaledElapsedTime()
        {
            return Time.unscaledTime - m_StartTime;
        }
        float GetScaledElapsedTime()
        {
            return Time.time - m_StartTime;
        }

        public virtual void Begin()
        {
            if (m_Transform == null)
            {
                return;
            }

            if (m_UseUnscaledTime)
            {
                m_StartTime = Time.unscaledTime;
                m_GetElapsedTime = GetUnscaledElapsedTime;
            }
            else
            {
                m_StartTime = Time.time;
                m_GetElapsedTime = GetScaledElapsedTime;
            }
            
            m_Running = true;
        }

        public virtual void End()
        {
            m_Running = false;
        }

        void Update()
        {
            if (m_Running)
            {
                Modify(m_GetElapsedTime());
            }
        }

        protected virtual void Modify(float inElapsedTime)
        {
            /// Position
            Vector3 tPosition = (m_Space == Space.World) ? m_Transform.position : m_Transform.localPosition;
            if (Pos_X != null)
            {
                tPosition.x = Pos_X.Evaluate(inElapsedTime);
            }
            if (Pos_Y != null)
            {
                tPosition.y = Pos_Y.Evaluate(inElapsedTime);
            }
            if (Pos_Z != null)
            {
                tPosition.z = Pos_Z.Evaluate(inElapsedTime);
            }
            m_Transform.Translate(tPosition, m_Space);

            /// Rotation
            Vector3 tEulerAngles = (m_Space == Space.World) ? m_Transform.eulerAngles : m_Transform.localEulerAngles;
            if (Rot_X != null)
            {
                tEulerAngles.x = Rot_X.Evaluate(inElapsedTime);
            }
            if (Rot_Y != null)
            {
                tEulerAngles.y = Rot_Y.Evaluate(inElapsedTime);
            }
            if (Rot_Z != null)
            {
                tEulerAngles.z = Rot_Z.Evaluate(inElapsedTime);
            }
            m_Transform.Rotate(tPosition, m_Space);

            /// Scale
            Vector3 tScale = m_Transform.localScale;
            if (Scale_U != null)
            {
                tScale.x = tScale.y = tScale.z = Scale_U.Evaluate(inElapsedTime);
            }
            else
            {
                if (Scale_X != null)
                {
                    tScale.x = Scale_X.Evaluate(inElapsedTime);
                }
                if (Scale_Y != null)
                {
                    tScale.y = Scale_Y.Evaluate(inElapsedTime);
                }
                if (Scale_Z != null)
                {
                    tScale.z = Scale_Z.Evaluate(inElapsedTime);
                }
            }
            m_Transform.localScale = tScale;
        }

        public Space m_Space = Space.Self;

        public AnimationCurve Pos_X = null;
        public AnimationCurve Pos_Y = null;
        public AnimationCurve Pos_Z = null;

        public AnimationCurve Rot_X = null;
        public AnimationCurve Rot_Y = null;
        public AnimationCurve Rot_Z = null;

        public bool UseUniqueScale = true;

        public AnimationCurve Scale_U = null;

        public AnimationCurve Scale_X = null;
        public AnimationCurve Scale_Y = null;
        public AnimationCurve Scale_Z = null;
    }
}
