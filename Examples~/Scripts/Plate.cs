using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.Attributes.Example
{
    public sealed class Plate : MonoBehaviour
    {
        public event BallGoalReachDelegate onBallGoalReach;

        [SerializeField]
        [NotNull]
        private List<Transform> m_spawnPoints = default;

        public IReadOnlyList<Transform> spawnPoints => m_spawnPoints;

        [SerializeField]
        [NotNull]
        private List<Goal> m_goals = default;

        private void Awake()
        {
            m_spawnPoints ??= new List<Transform>();
            m_goals ??= new List<Goal>();
        }

        private void OnEnable()
        {
            foreach (var goal in m_goals)
            {
                goal.onBallReach += OnBallGoalReach;
            }
        }

        private void OnDisable()
        {
            foreach (var goal in m_goals)
            {
                goal.onBallReach -= OnBallGoalReach;
            }
        }

        private void OnBallGoalReach(in Goal goal, in Ball ball)
        {
            onBallGoalReach?.Invoke(goal, ball);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_spawnPoints == null) return;
            foreach (var point in m_spawnPoints)
            {
                if (point == null) continue;
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(point.position, 0.25f);
                Gizmos.DrawLine(point.position, point.position + point.forward * 0.5f);
            }
        }
#endif
    }
}
