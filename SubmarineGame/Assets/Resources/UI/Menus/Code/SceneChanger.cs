using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenuPanel;
    private bool m_isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        m_isPaused = !m_isPaused;

        if (m_isPaused)
        {
            Time.timeScale = 0f; // Freeze the game
            if (m_pauseMenuPanel != null) m_pauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            if (m_pauseMenuPanel != null) m_pauseMenuPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // Ensure time is running when starting
        SceneManager.LoadScene("GameLevel");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void ToggleObject(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
    }
}