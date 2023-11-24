using Unity.VisualScripting;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject MainMenuLoader;
    [SerializeField] private GameObject EnemySpawner;
    [SerializeField] private GameObject AllMenu;
    [SerializeField] private GameObject MainMenuPosition;
    private static Vector3 newPosition;
    [SerializeField] private bool CanMove=false ,ShouldMove=false;
    [SerializeField] private GameObject OptionsSection;
    [SerializeField] private GameObject AchievementsSection;
    private void Update()
    {
        if (!CanMove) 
            return;

        if (ShouldMove)
        {
            AllMenu.transform.position = Vector3.Lerp(AllMenu.transform.position, newPosition, 2f * Time.deltaTime);
        }
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }
    public void PlayButton()
    {
        OptionsSection.SetActive(false);
        AchievementsSection.SetActive(false);
        EnemySpawner.GetComponent<MenuEnemySpawner>().isActive = false;
        MainMenuLoader.GetComponent<MainMenuScripts>().TimerIsActive = true;
        MainCamera.GetComponent<Animation>().Play("CameraMainMenuBack");
    }
    public void Options()
    {
        OptionsSection.SetActive(true);
        float newX = MainMenuPosition.transform.position.x + 3;
        ShouldMove = true;
        newPosition = new Vector3(newX, MainMenuPosition.transform.position.y, MainMenuPosition.transform.position.z);
    }
    public void Achievements()
    {
        AchievementsSection.SetActive(true);
        float newX = MainMenuPosition.transform.position.x - 3;
        ShouldMove = true;
        newPosition = new Vector3(newX, MainMenuPosition.transform.position.y, MainMenuPosition.transform.position.z);
    }
    public void ExitToMain()
    {
        newPosition = new Vector3(MainMenuPosition.transform.position.x, MainMenuPosition.transform.position.y, MainMenuPosition.transform.position.z);
    }
    private void OnEnable()
    {
        if(CanMove)
        {
            AllMenu.transform.position = Vector3.Lerp(AllMenu.transform.position, MainMenuPosition.transform.position, 1000f);
            ShouldMove = true;
            newPosition = MainMenuPosition.transform.position;
        }
    }
}
