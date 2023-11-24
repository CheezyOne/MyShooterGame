using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public static Action onExit;
    [SerializeField] private GameObject Player;
    [SerializeField] private int Level;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            try
            {
                Cursor.lockState = CursorLockMode.None;
                LevelUnlocks.UnlockedLevels[Level] = true;
                SceneManager.LoadScene("MainMenu");
            }
            catch
            {

            }
        }
    }
}
