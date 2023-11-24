using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsEscPress : MonoBehaviour
{

    private ButtonManager ButtonsScript;
    private void Start()
    {
        ButtonsScript = GetComponent<ButtonManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonsScript.BackToMainMenu();
        }
    }
}
