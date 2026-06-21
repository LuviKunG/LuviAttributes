using UnityEngine;

namespace LuviKunG.Attributes.Example
{
    [RequireComponent(typeof(Collider))]
    public class Goal : MonoBehaviour
    {
        [SerializeField]
        private Collider m_collider = default;

        public event BallGoalReachDelegate onBallReach;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                onBallReach?.Invoke(this, ball);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            m_collider = GetComponent<Collider>();
            if (m_collider != null)
            {
                m_collider.isTrigger = true;
            }
        }
#endif
    }
}
