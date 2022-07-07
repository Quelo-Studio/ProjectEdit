using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 200f;

    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5.0f);
    }

    private void FixedUpdate() => m_Rigidbody.velocity = transform.right * m_Speed * Time.fixedDeltaTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
