using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsEscPress : MonoBehaviour
{
    private ButtonManager ButtonsScript;
    private void Start()
    {
        ButtonsScript=GetComponent<ButtonManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonsScript.BackToWeapons();
        }
    }
}
