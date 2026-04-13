using UnityEngine;

public class HookController : MonoBehaviour
{

    [SerializeField] private float m_hook_length;
    private Vector3 m_hook_end_loc;
    private DistanceJoint2D m_hook_joint;
    private bool m_hook_is_active;

    [SerializeField] private LineRenderer m_hook_rope;

    private GameObject m_hook_held_item;

    [SerializeField] private LayerMask m_hookable_layer;    // This would be the treasure layer specifically

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        m_hook_joint = this.gameObject.GetComponent<DistanceJoint2D>();

        m_hook_joint.enabled = false;   // hook not visible until shot
        m_hook_rope.enabled = false;    // Generate rope only when treasure is held

    }

    // Update is called once per frame
    void Update()
    {

        hookCheck();

    }

    public bool isHookActive() { return m_hook_is_active; }

    public GameObject getHookHeldItem() { return m_hook_held_item; }

    private void setHookRope( Vector3 start_point, Vector3 end_point ) {
    
        m_hook_rope.SetPosition(0, start_point);
        m_hook_rope.SetPosition(1, end_point);

    }

    /* Brief: Toggle hook on and off and connects with target treasure
     *
     * - On mouse press:
     *   - If holding treasure:
     *      - releases treasure
     *   - If not holding treasure
     *      - raycasts for treasure and captures it with a joint connection
    */
    private void hookCheck() {

        // If hook isn't already holding an item and mouse is clicked
        // Shoot raycast to see if we hit a treasure
        // Grab the treasure and connect a joint to it (to hold it)

        if ( Input.GetMouseButtonDown(0) ) {

            Debug.Log("Mouse button down");

            if ( !m_hook_is_active ) {

                Debug.Log("Hook shot");

                RaycastHit2D hook_hit = Physics2D.Raycast(

                    Camera.main.ScreenToWorldPoint(Input.mousePosition),        // origin
                    Vector2.zero,                                               // direction (not needed)
                    Mathf.Infinity,                                             // distance
                    m_hookable_layer                                           // LayerMask

                );

                if ( hook_hit.collider != null ) {

                    Debug.Log("Hook collided with treasure");

                    m_hook_held_item = hook_hit.collider.gameObject;    // This should be a treasure we want to hold onto
                    m_hook_end_loc = hook_hit.point;                    // Where the hook connects onto the treasure
                    m_hook_end_loc.z = 0;                               // We don't want to mess with z axis in 2D game

                    m_hook_joint.connectedAnchor = m_hook_end_loc;
                    // m_hook_joint.connectedBody = m_hook_held_item.GetComponent<Rigidbody2D>();
                    m_hook_joint.enabled = true;
                    m_hook_joint.distance = m_hook_length;

                    m_hook_is_active = true;

                    setHookRope(m_hook_held_item.transform.position, transform.position);
                    m_hook_rope.enabled = true;

                }

            }

            // If the hook is already active (i.e. holding an item)
            // Then mouse click releases the item

            else {

                m_hook_joint.enabled = false;
                m_hook_rope.enabled = false;

            }

        }

        if ( m_hook_rope.enabled ) {
                setHookRope(m_hook_held_item.transform.position, transform.position);
                // m_hook_joint.anchor = transform.poisition;
                // m_hook_joint.connectedAnchor = m_hook_held_item.transform.poisition;
        }

    }

}
