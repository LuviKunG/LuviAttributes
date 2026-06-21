using UnityEngine;

namespace LuviKunG.Attributes.Example
{

    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public event BallFallDelegate onFall;

        [SerializeField]
        private float m_fallY = -10.0f;

        private void Update()
        {
            if (transform.position.y < m_fallY)
            {
                onFall?.Invoke(this);
                enabled = false;
            }
        }
    }
}
