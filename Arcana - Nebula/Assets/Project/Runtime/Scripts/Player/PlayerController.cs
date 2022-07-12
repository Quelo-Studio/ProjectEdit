using UnityEngine;

namespace ArcaneNebula
{
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(PlayerCombat))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMotor m_Motor;
        private PlayerCombat m_Combat;

        private float m_Move = 0.0f;

        private void Start()
        {
            m_Motor = GetComponent<PlayerMotor>();
            m_Combat = GetComponent<PlayerCombat>();

            InputSystem.Player.Jump.performed += m_Motor.OnJump;
            InputSystem.Player.Jump.performed += m_Motor.OnWallJump;
            InputSystem.Player.Jump.canceled += m_Motor.OnJumpStop;

            InputSystem.Player.Dash.performed += m_Motor.OnDash;
        }

        private void Update()
        {
            m_Move = InputSystem.Player.Move.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            m_Motor.Move(m_Move);
        }
    }
}
