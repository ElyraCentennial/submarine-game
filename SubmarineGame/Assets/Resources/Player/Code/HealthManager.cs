using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int m_lives = 3;
    [SerializeField] private float m_invulnerabilityDuration = 1.5f;
    private bool m_isInvulnerable = false;

    private SpriteRenderer m_submarine_sprite;

    public UnityEvent onGameOver;

    void Start()
    {
        m_submarine_sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (m_isInvulnerable || m_lives <= 0) return;

        m_lives--;
        
        if (m_lives <= 0)
        {
            onGameOver?.Invoke();
            
            // Disable movement when game is over
            SubmarineMovement movement = GetComponent<SubmarineMovement>();
            if (movement != null)
            {
                movement.enabled = false;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        m_isInvulnerable = true;
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < m_invulnerabilityDuration)
        {
            // Toggle visibility
            isVisible = !isVisible;
            if (m_submarine_sprite != null)
            {
                m_submarine_sprite.enabled = isVisible;
            }

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        // Ensure sprite is visible at the end
        if (m_submarine_sprite != null)
        {
            m_submarine_sprite.enabled = true;
        }
        m_isInvulnerable = false;
    }
}
