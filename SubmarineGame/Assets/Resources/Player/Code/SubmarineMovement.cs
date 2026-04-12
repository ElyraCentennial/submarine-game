using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineMovement : MonoBehaviour
{

    private Rigidbody2D m_submarine_rb;
    private CapsuleCollider2D m_submarine_collider;

    [SerializeField] private float m_move_speed;
    private Vector2 m_move_direction;

    [SerializeField] private InputActionReference m_move_input;
    [SerializeField] private InputActionReference m_hook_toggle;

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

    }

    private void FixedUpdate() {

        m_submarine_rb.linearVelocity = new Vector2(
                                m_move_direction.x * m_move_speed,
                                m_move_direction.y * m_move_speed
                            );

    }

}
