using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerController : MonoBehaviour
{
    private PlayerMotor m_Motor;
    private PlayerCombat m_Combat;

    private float m_Move = 0.0f;
    private bool m_Jump = false;
    private bool m_JumpHold = false;
    private bool m_JumpStop = false;

    private void Start()
    {
        m_Motor = GetComponent<PlayerMotor>();
        m_Combat = GetComponent<PlayerCombat>();
    }
    private void Update()
    {
        if (!m_Jump)
            m_Jump = Input.GetButtonDown("Jump");

        m_JumpHold = Input.GetButton("Jump");

        if (!m_JumpStop)
            m_JumpStop = Input.GetButtonUp("Jump");

        m_Move = Input.GetAxis("Horizontal");

        m_Motor.Animate(m_Move);
        if (Input.GetKeyDown(KeyCode.X))
            m_Combat.SetAttack();
    }

    private void FixedUpdate()
    {
        m_Motor.Move(m_Move, m_Jump, m_JumpHold, m_JumpStop);

        m_Jump = false;
        m_JumpStop = false;
    }
}
