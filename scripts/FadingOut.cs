using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingOut : MonoBehaviour
{
    private Animation ScreenAnimation;
    private float TimeUntilMouseActivation = 1f;
    private bool MouseIsActive = false;
    private void Awake()
    {
        MouseLook.isAbleToMoveMouse = false;
        ScreenAnimation = GetComponent<Animation>();
        ScreenAnimation.Play("FadingOutScreen");
    }
    private void Update()
    {
        if (MouseIsActive)
            return;
        if(TimeUntilMouseActivation>0)
        {
            TimeUntilMouseActivation -= Time.deltaTime;
            return;
        }
        MouseIsActive = true;
        MouseLook.isAbleToMoveMouse = true;
    }
    public void CanvasDisable()
    {
        gameObject.SetActive(false);
    }
}
