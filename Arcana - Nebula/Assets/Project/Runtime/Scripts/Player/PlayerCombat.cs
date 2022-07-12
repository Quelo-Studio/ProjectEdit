using UnityEngine;

namespace ArcaneNebula
{
    public class PlayerCombat : MonoBehaviour
    {
        [HideInInspector] public bool IsAttacking { get { return m_NextAttackTime > Time.time; } }

        [SerializeField] private float m_AttackRadius = 0.3f;
        [SerializeField] private LayerMask m_EnemiesLayer;
        [SerializeField] private Attack[] m_ComboAttacks;

        private Animator m_Animator;

        private float m_NextAttackTime;
        private int m_CurrentAttackIndex;
        private float m_AttackExitTime;

        private void Start()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (m_AttackExitTime > 0)
                m_AttackExitTime -= Time.deltaTime;
            else
            {
                m_AttackExitTime = 0;
                m_CurrentAttackIndex = 0;
            }
        }

        public void SetAttack()
        {
            if (Time.time > m_NextAttackTime)
            {
                m_Animator.SetTrigger(m_ComboAttacks[m_CurrentAttackIndex].AttackAnimation.name);

                m_AttackExitTime = m_ComboAttacks[m_CurrentAttackIndex].AttackAnimation.length + 0.1f;
                m_NextAttackTime = Time.time + m_ComboAttacks[m_CurrentAttackIndex].AttackAnimation.length - 0.1f;
            }
        }

        public void PerformAttack()
        {
            foreach (Transform transform in m_ComboAttacks[m_CurrentAttackIndex].AttackPoints)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, m_AttackRadius, m_EnemiesLayer);
                foreach (Collider2D eCollider in enemies)
                    eCollider.GetComponent<Enemy>()?.TakeDamage(10);
            }

            m_CurrentAttackIndex++;
            if (m_CurrentAttackIndex >= m_ComboAttacks.Length)
                m_CurrentAttackIndex = 0;
        }

        private void OnDrawGizmosSelected()
        {
            /*        if (m_ComboAttacks[m_CurrentAttackIndex].AttackPoints != null)
                    {
                        foreach (Transform transform in m_ComboAttacks[m_CurrentAttackIndex].AttackPoints)
                        {
                            if (transform)
                                Gizmos.DrawWireSphere(transform.position, m_AttackRadius);
                        }
                    }*/
        }
    }

    [System.Serializable]
    public class Attack
    {
        public AnimationClip AttackAnimation;
        public Transform[] AttackPoints;
    }
}
