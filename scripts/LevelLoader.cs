using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] Levels;
    private int LevelNumber;
    private void Awake()
    {
        LevelNumber = DataHolder.LevelNumber;
        Levels[LevelNumber].SetActive(true);
    }
}
