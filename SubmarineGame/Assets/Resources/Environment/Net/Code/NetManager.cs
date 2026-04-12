using UnityEngine;

public class NetManager : MonoBehaviour
{

    [SerializeField] private LayerMask m_submarine_layer;
    [SerializeField] private LayerMask m_net_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Physics2D.IgnoreLayerCollision(9, 11, true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
