using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;
    [SerializeField] private Transform m_Player;

    private int m_CurHealth;

    private Animator m_Animator;

    private void Start()
    {
        m_CurHealth = m_MaxHealth;
        m_Animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        m_Animator.SetTrigger("Take Hit");

        m_CurHealth -= damage;
        if (m_CurHealth <= 0)
        {
            m_Animator.SetBool("IsDead", true);
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;
        }
    }

    public void Flip()
    {
        if (m_Player.position.x > transform.position.x && transform.eulerAngles.y == 180.0f)
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        else if (m_Player.position.x < transform.position.x && transform.eulerAngles.y == 0.0f)
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    }
}
