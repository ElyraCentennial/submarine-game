using UnityEngine;
using UnityEngine.Events;

public class NetManager : MonoBehaviour
{
    [SerializeField] private int m_requiredTreasures = 3;
    private int m_currentTreasures = 0;

    [Header("UI Settings")]
    [SerializeField] private GameObject m_winPanel;

    public UnityEvent onGameWon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Treasure"))
        {
            m_currentTreasures++;
            CheckWinCondition();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Treasure"))
        {
            m_currentTreasures--;
        }
    }

    private void CheckWinCondition()
    {
        if (m_currentTreasures >= m_requiredTreasures)
        {

            if (m_winPanel != null) m_winPanel.SetActive(true);

            Time.timeScale = 0f;

            onGameWon?.Invoke();
        }
    }
}