using UnityEngine;

public class MainMenuScripts : MonoBehaviour
{
    private bool isActivated = false;
    private float Timer = 1.65f;
    public bool TimerIsActive = false;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject LevelsMenu;
    private CanvasGroup CG;
    private bool isGoingToMainMenu = false;
    private bool CheckOnlyOnce = false;
    [SerializeField] private GameObject[] MainButtons;
    private Color[] MainButtonsColor;
    void Start()
    {
        MainButtonsColor= new Color[MainButtons.Length];
        for (int i=0;  i<MainButtons.Length;i++)
        {
            MainButtonsColor[i]= MainButtons[i].GetComponent<SpriteRenderer>().color;
        }
        CG = mainMenu.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (TimerIsActive)
        {
            if (!CheckOnlyOnce)
            {
                if (mainMenu.activeInHierarchy)
                {
                    CheckOnlyOnce = true;
                    isGoingToMainMenu = false;
                    isActivated = false;
                }
                else
                {
                    CheckOnlyOnce = true;
                    isGoingToMainMenu = true;
                }
            }
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                if (isGoingToMainMenu)
                {
                    isActivated = true;
                    mainMenu.SetActive(true);
                }
                else
                {
                    ButtonManager.LevelsDisappear[0] = false;
                    LevelsMenu.SetActive(true);
                    MenuObjectsFall.ShouldFall = false;
                }    
                CheckOnlyOnce = false;
                Timer = 1.65f;
                TimerIsActive = false;
            }
        }
        if(MainButtonsColor[0].a < 0.8f && isActivated)
        {
            for (int i = 0; i < MainButtons.Length; i++)
                {
                    MainButtonsColor[i] += new Color(0, 0, 0, 0.01f);
                    MainButtons[i].GetComponent<SpriteRenderer>().color = MainButtonsColor[i];
                }
        }
        else if (MainButtonsColor[0].a > 0 && !isActivated)
        {
            for (int i = 0; i < MainButtons.Length; i++)
            {
                MainButtonsColor[i] -= new Color(0, 0, 0, 0.02f);
                MainButtons[i].GetComponent<SpriteRenderer>().color = MainButtonsColor[i];
            }
        }
        if (isActivated && CG.alpha < 1)
        { 
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, new Vector3(mainMenu.transform.position.x, mainMenu.transform.position.y, mainMenu.transform.position.z - 0.2f), Time.deltaTime / 2);
            CG.alpha += Time.deltaTime*2;
        }
        else if (!isActivated && CG.alpha > 0)
        {
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, new Vector3(mainMenu.transform.position.x, mainMenu.transform.position.y, mainMenu.transform.position.z + 0.2f), Time.deltaTime / 2);
            CG.alpha -= Time.deltaTime*2;
        }
        else  if (CG.alpha <= 0 && !isActivated)
        {
            mainMenu.SetActive(false);
        }
    }
}
