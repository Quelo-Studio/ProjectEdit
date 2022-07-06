using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float m_Speed = 100.0f;
    [SerializeField] private float m_JumpForce = 80.0f;
    [SerializeField] private float m_JumpTime = 1.0f;

    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Vector2 m_GroundCheckSize = new Vector2(0.5f, 0.02f);
    [SerializeField] private LayerMask m_GroundLayer;

    private Rigidbody2D m_Rigidbody2D;
    private Animator m_Animator;
    private PlayerCombat m_PlayerCombat;

    private bool m_IsGrounded = false;
    private bool m_IsJumping = false;
    private bool m_FacingRight = true;

    private float m_JumpTimeCounter;

    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        m_PlayerCombat = GetComponent<PlayerCombat>();

        m_JumpTimeCounter = m_JumpTime;
    }

    private void FixedUpdate()
    {
        m_IsGrounded = Physics2D.OverlapBox(m_GroundCheck.position, m_GroundCheckSize, 0.0f, m_GroundLayer);
    }

    public void Move(float move, bool jump, bool jumpHold, bool jumpStop)
    {
        if (jumpStop)
            m_IsJumping = false;

        bool first = false;

        m_Rigidbody2D.velocity = new Vector2(move * m_Speed * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);

        if (jump && m_IsGrounded)
        {
            m_IsGrounded = false;
            first = true;
            m_IsJumping = true;
            m_JumpTimeCounter = m_JumpTime;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * Time.fixedDeltaTime);
        }

        if (!m_PlayerCombat.IsAttacking)
        {
            if (m_FacingRight && move < 0.0f) Flip();
            else if (!m_FacingRight && move > 0.0f) Flip();
        }

        if (jumpHold && m_IsJumping && !first)
        {
            if (m_JumpTimeCounter > 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * Time.fixedDeltaTime);
                m_JumpTimeCounter -= Time.fixedDeltaTime;
            }
            else m_IsJumping = false;
        }
    }

    public void Animate(float move)
    {
        m_Animator.SetFloat("Speed", Mathf.Abs(move));
        m_Animator.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        m_Animator.SetBool("IsGrounded", m_IsGrounded);
    }

    private void Flip()
    {
        transform.rotation = transform.eulerAngles.y == 0.0f ? Quaternion.Euler(0.0f, 180.0f, 0.0f) : Quaternion.Euler(0.0f, 0.0f, 0.0f);
        m_FacingRight = !m_FacingRight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(m_GroundCheck.position, m_GroundCheckSize);
    }
}
