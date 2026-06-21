using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LuviKunG.Attributes.Example
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        [PrefabAsset]
        private Plate m_prefabPlate = default;

        [SerializeField]
        [PrefabAsset]
        private Ball m_prefabBall = default;

        [SerializeField]
        private float m_rotationSpeed = 50f;

        [SerializeField]
        private float m_maxTiltAngle = 35f;

        [SerializeField]
        private InputAction m_mouseAxisAction = new(
            name: "MouseAxis",
            type: InputActionType.Value,
            binding: "<Mouse>/delta",
            expectedControlType: "Vector2"
        );

        private Plate m_plateInstance;
        private readonly List<Ball> m_ballInstances;

        [SerializeField]
        [ReadOnly]
        private float m_tiltX;
        [SerializeField]
        [ReadOnly]
        private float m_tiltZ;

        public GameController() : base()
        {
            m_ballInstances = new List<Ball>();
        }

        private void OnEnable()
        {
            m_mouseAxisAction.Enable();
        }

        private void OnDisable()
        {
            m_mouseAxisAction.Disable();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (m_prefabPlate == null)
                return;
            Plate newPlate = Instantiate(m_prefabPlate);
            newPlate.onBallGoalReach += OnBallGoalReached;
            m_plateInstance = newPlate;
            m_ballInstances.Clear();
            foreach (var transform in m_plateInstance.spawnPoints)
            {
                Ball newBall = Instantiate(m_prefabBall, transform.position, transform.rotation);
                newBall.gameObject.SetActive(true);
                newBall.onFall += OnBallFall;
                m_ballInstances.Add(newBall);
            }
            m_tiltX = 0f;
            m_tiltZ = 0f;
        }

        private void Update()
        {
            if (m_plateInstance == null)
                return;
            Vector2 delta = m_mouseAxisAction.ReadValue<Vector2>();
            m_tiltX = Mathf.Clamp(m_tiltX + delta.y * m_rotationSpeed * Time.deltaTime, -m_maxTiltAngle, m_maxTiltAngle);
            m_tiltZ = Mathf.Clamp(m_tiltZ - delta.x * m_rotationSpeed * Time.deltaTime, -m_maxTiltAngle, m_maxTiltAngle);
            m_plateInstance.transform.rotation = Quaternion.Euler(m_tiltX, 0f, m_tiltZ);
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void CheckGameplayCondition()
        {
            int activeBallCount = GetActiveBallCount();
            if (activeBallCount == 0)
            {
                RestartGame();
            }
        }

        private int GetActiveBallCount()
        {
            int activeBallCount = 0;
            foreach (Ball ball in m_ballInstances)
            {
                if (ball.gameObject.activeSelf)
                {
                    activeBallCount++;
                }
            }
            return activeBallCount;
        }

        private void OnBallFall(in Ball ball)
        {
            ball.gameObject.SetActive(false);
            CheckGameplayCondition();
        }

        private void OnBallGoalReached(in Goal goal, in Ball ball)
        {
            ball.gameObject.SetActive(false);
            CheckGameplayCondition();
        }
    }
}
