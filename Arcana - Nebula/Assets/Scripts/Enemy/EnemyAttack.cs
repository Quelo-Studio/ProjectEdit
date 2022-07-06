using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Offset = 90f;

    private bool m_Attack = false;

    private EnemyAI m_EnemyAI;

    private void Start()
    {
        m_EnemyAI = GetComponent<EnemyAI>();   
    }

    private void Update()
    {
        if (m_EnemyAI.ReachedEndOfPath && !m_Attack)
        {
            m_Attack = true;
            Vector3 direction = (transform.position - m_Target.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Instantiate(m_Projectile, transform.position, Quaternion.Euler(new Vector3(0f, 0f, -angle - m_Offset)));
        }
        else if (!m_EnemyAI.ReachedEndOfPath)
            m_Attack = false;
    }
}
