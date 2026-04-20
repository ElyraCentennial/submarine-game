using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HookController : MonoBehaviour
{
    [SerializeField] private float m_hook_length;
    private Vector3 m_hook_end_loc;
    private DistanceJoint2D m_hook_joint;
    private bool m_hook_is_active;

    [SerializeField] private LineRenderer m_hook_rope;
    private GameObject m_hook_held_item;

    [SerializeField] private LayerMask m_hookable_layer;

    [Header("UI Display")]
    [SerializeField] private TextMeshProUGUI m_itemDisplayUI;

    private Dictionary<string, string> m_itemNames = new Dictionary<string, string>();

    void Start()
    {
        m_hook_joint = this.gameObject.GetComponent<DistanceJoint2D>();
        m_hook_joint.enabled = false;
        m_hook_rope.enabled = false;

        m_itemNames.Add("Treasure", "Chest");
    }

    void Update()
    {
        hookCheck();
    }

    public bool isHookActive() { return m_hook_is_active; }

    public GameObject getHookHeldItem() { return m_hook_held_item; }

    private void setHookRope(Vector3 start_point, Vector3 end_point)
    {
        m_hook_rope.SetPosition(0, start_point);
        m_hook_rope.SetPosition(1, end_point);
    }

    private void hookCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_hook_is_active)
            {
                RaycastHit2D hook_hit = Physics2D.Raycast(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero,
                    Mathf.Infinity,
                    m_hookable_layer
                );

                if (hook_hit.collider != null)
                {
                    m_hook_held_item = hook_hit.collider.gameObject;
                    m_hook_end_loc = hook_hit.point;
                    m_hook_end_loc.z = 0;

                    string itemTag = m_hook_held_item.tag;
                    if (m_itemNames.ContainsKey(itemTag))
                    {
                        m_itemDisplayUI.text = m_itemNames[itemTag];
                    }

                    Rigidbody2D hitRb = m_hook_held_item.GetComponent<Rigidbody2D>();
                    if (hitRb != null)
                    {
                        m_hook_joint.connectedBody = hitRb;
                        m_hook_joint.autoConfigureConnectedAnchor = false;
                        m_hook_joint.connectedAnchor = m_hook_held_item.transform.InverseTransformPoint(m_hook_end_loc);
                    }
                    else
                    {
                        m_hook_joint.connectedBody = null;
                        m_hook_joint.connectedAnchor = m_hook_end_loc;
                    }
                    m_hook_joint.enabled = true;
                    m_hook_joint.distance = m_hook_length;
                    m_hook_is_active = true;
                    setHookRope(m_hook_held_item.transform.position, transform.position);
                    m_hook_rope.enabled = true;
                }
            }
            else
            {
                if (m_itemDisplayUI != null) m_itemDisplayUI.text = "";

                m_hook_joint.enabled = false;
                m_hook_rope.enabled = false;
                m_hook_joint.connectedBody = null;
                m_hook_is_active = false;
            }
        }

        if (m_hook_rope.enabled)
        {
            setHookRope(m_hook_held_item.transform.position, transform.position);
        }
    }
}
