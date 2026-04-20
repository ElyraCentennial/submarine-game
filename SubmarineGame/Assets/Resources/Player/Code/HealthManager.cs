using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int m_lives = 3;
    [SerializeField] private float m_invulnerabilityDuration = 1.5f;
    private bool m_isInvulnerable = false;

    [Header("UI Settings")]
    [SerializeField] private Image[] m_heartImages;
    [SerializeField] private GameObject m_gameOverPanel;

    [Header("Screen Shake")]
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private float m_shakeDuration = 0.2f;
    [SerializeField] private float m_shakeMagnitude = 0.1f;
    private Vector3 m_originalCameraPos;

    private SpriteRenderer m_submarine_sprite;

    public UnityEvent onGameOver;

    void Start()
    {
        m_submarine_sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (m_cameraTransform == null && Camera.main != null)
        {
            m_cameraTransform = Camera.main.transform;
        }
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

        if (m_cameraTransform != null)
        {
            StartCoroutine(Shake());
        }

        if (m_heartImages != null && m_lives < m_heartImages.Length)
        {
            m_heartImages[m_lives].gameObject.SetActive(false);
        }

        if (m_lives <= 0)
        {
            // Show Game Over UI
            if (m_gameOverPanel != null) m_gameOverPanel.SetActive(true);

            // Freeze the game
            Time.timeScale = 0f;

            onGameOver?.Invoke();

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

    private IEnumerator Shake()
    {
        m_originalCameraPos = m_cameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < m_shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * m_shakeMagnitude;
            float y = Random.Range(-1f, 1f) * m_shakeMagnitude;
            m_cameraTransform.localPosition = new Vector3(m_originalCameraPos.x + x, m_originalCameraPos.y + y, m_originalCameraPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        m_cameraTransform.localPosition = m_originalCameraPos;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        m_isInvulnerable = true;
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < m_invulnerabilityDuration)
        {
            isVisible = !isVisible;
            if (m_submarine_sprite != null)
            {
                m_submarine_sprite.enabled = isVisible;
            }
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (m_submarine_sprite != null)
        {
            m_submarine_sprite.enabled = true;
        }
        m_isInvulnerable = false;
    }
}