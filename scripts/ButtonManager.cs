using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static Action onLevelSelection;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject MenuEnemySpawner;
    [SerializeField] private GameObject MainMenuLoader;
    [SerializeField] private int LevelNum;
    [SerializeField] private GameObject[] Levels;
    public static bool[] LevelsDisappear= { false, true, true, true, true };
    private static CanvasGroup[] CGs = new CanvasGroup[5];
    public bool NeedCanvasGroup = true;
    private void Start()
    {
        
        if (!NeedCanvasGroup)
            return;
        for (int i = 0; i < LevelsDisappear.Length; i++)
        {
            CGs[i] = Levels[i].GetComponent<CanvasGroup>();
        }

    }
    private void Update()
    {
        if (!NeedCanvasGroup)
            return;
        for (int i=0;i< LevelsDisappear.Length; i++)
        {
            if (!LevelsDisappear[i] && CGs[i].alpha < 1)
            {
                Levels[i].transform.position = Vector3.Lerp(Levels[i].transform.position, new Vector3(Levels[i].transform.position.x, Levels[i].transform.position.y, Levels[i].transform.position.z - 0.2f), Time.deltaTime);
                CGs[i].alpha += Time.deltaTime ;
            }
            if (LevelsDisappear[i] && CGs[i].alpha > 0)
            {
                Levels[i].transform.position = Vector3.Lerp(Levels[i].transform.position, new Vector3(Levels[i].transform.position.x, Levels[i].transform.position.y, Levels[i].transform.position.z + 0.2f), Time.deltaTime);
                CGs[i].alpha -= Time.deltaTime ;
            }
            if (CGs[i].alpha<=0 && LevelsDisappear[i])
            {
                Levels[i].SetActive(false);
            }
        }
    }
    public void PistolBoi()
    {
        MenuObjectsFall.ShouldFall = true;
        if (LevelsDisappear[0])
            return;
        LevelsDisappear[0] = true;
        LevelsDisappear[1] = false;
        Levels[1].SetActive(true);
        DataHolder.ChosenCharacter = "Pistolboi";
    }
    public void ShotgunBoi()
    {
        MenuObjectsFall.ShouldFall = true;
        if (LevelsDisappear[0])
            return;
        LevelsDisappear[0] = true;
        LevelsDisappear[4] = false;
        Levels[4].SetActive(true);
        DataHolder.ChosenCharacter = "ShotgunBoi";
    }
    public void MachinegunBoi()
    {
        MenuObjectsFall.ShouldFall = true;
        if (LevelsDisappear[0])
            return;
        LevelsDisappear[0] = true;
        LevelsDisappear[3] = false;
        Levels[3].SetActive(true);
        DataHolder.ChosenCharacter = "MachinegunBoi";
    }
    public void SniperBoi()
    {
        MenuObjectsFall.ShouldFall = true;
        if (LevelsDisappear[0])
            return;
        LevelsDisappear[0] = true;
        LevelsDisappear[2] = false;
        Levels[2].SetActive(true);
        DataHolder.ChosenCharacter = "SniperBoi";
    }
    public void Retry()
    {
        SceneManager.LoadScene("Map");
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void BackToWeapons()
    {
        MenuObjectsFall.ShouldFall = false;
        LevelsDisappear[0] = false;
        Levels[0].SetActive(true);
        for (int j = 1; j < 5; j++)
        {
            LevelsDisappear[j] = true;
        }
    }
    public void BackToMainMenu()
    {
        LevelsDisappear[0] = true;
        MenuObjectsFall.ShouldFall = true;
        Camera.GetComponent<Animation>().Play("CameraMainMenu");
        MenuEnemySpawner.GetComponent<MenuEnemySpawner>().isActive = true;
        MainMenuLoader.GetComponent<MainMenuScripts>().TimerIsActive = true;
    }
    public void LoadALevel()
    {
        if (!LevelUnlocks.UnlockedLevels[LevelNum - 1])
            return;
        LevelsDisappear[0] = false;
        for (int j = 1; j < LevelsDisappear.Length; j++)
        {
            LevelsDisappear[j] = true;
        }
        DataHolder.LevelNumber = LevelNum-1;
        onLevelSelection?.Invoke();
    }
}
