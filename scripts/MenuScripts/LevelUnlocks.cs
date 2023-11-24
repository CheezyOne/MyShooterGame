using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlocks : MonoBehaviour
{
    [SerializeField] private Color[] Colors;
    [SerializeField] private Image[] Levels;
    public static bool[] UnlockedLevels =new bool[16];
    private void Start()
    {
        UnlockedLevels[0] = true;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            for (int i = 0; i < UnlockedLevels.Length; i++)
            {
                if (UnlockedLevels[i])
                    UnlockLevel(i);
            }
        }
    }
    private void UnlockLevel(int Level)
    {
        Colors[Level].a = 0.65f;
        Levels[Level].color = Colors[Level];
    }

}
