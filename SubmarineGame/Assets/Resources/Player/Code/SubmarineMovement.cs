using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineMovement : MonoBehaviour
{

    private Rigidbody2D m_submarine_rb;
    private CapsuleCollider2D m_submarine_collider;

    [SerializeField] private float m_move_speed;
    private Vector2 m_move_direction;

    [SerializeField] private InputActionReference m_move_input;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem m_bubble_trail;
    private bool m_is_moving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        m_submarine_rb = this.gameObject.GetComponent<Rigidbody2D>();
        m_submarine_collider = this.gameObject.GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        m_move_direction = m_move_input.action.ReadValue<Vector2>();
        m_is_moving = m_move_direction.sqrMagnitude > 0.01f;

        HandleBubbles();

    }

    private void FixedUpdate() {

        Vector2 current_velocity = new Vector2(
                                m_move_direction.x * m_move_speed,
                                m_move_direction.y * m_move_speed
                            );

        m_submarine_rb.linearVelocity = current_velocity;

    }

    private void HandleBubbles()
    {
        if (m_bubble_trail != null)
        {
            if (m_is_moving && !m_bubble_trail.isPlaying)
            {
                m_bubble_trail.Play();
            }
            else if (!m_is_moving && m_bubble_trail.isPlaying)
            {
                m_bubble_trail.Stop();
            }
        }
    }

}
