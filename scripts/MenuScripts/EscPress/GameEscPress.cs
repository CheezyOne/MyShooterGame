using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEscPress : MonoBehaviour
{
    [SerializeField] private GameObject PauseCanvas;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }
    public void OpenCloseMenu()
    {
        if (PauseCanvas.activeSelf)
        {
            PauseCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
            return;
        }
        PauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Map");
    }
    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
