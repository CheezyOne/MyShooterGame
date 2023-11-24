using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChange : MonoBehaviour
{
    [SerializeField] private Text ResolutionText;
    private static int CurrentResolution = 6;
    private static bool FullScreen = true;
    private int[][] Resolutions;
    private int ResolutionsCount = 11;
    private void Awake()
    {
        Resolutions = new int[ResolutionsCount][];
        for (int i = 0; i < Resolutions.Length; i++)
            Resolutions[i] = new int[2];
        Resolutions[0][0] = 640;
        Resolutions[0][1] = 640;
        Resolutions[1][0] = 854; 
        Resolutions[1][1] = 480;
        Resolutions[2][0] = 960;
        Resolutions[2][1] = 540; 
        Resolutions[3][0] = 1280;
        Resolutions[3][1] = 720;
        Resolutions[4][0] = 1366; 
        Resolutions[4][1] = 768;
        Resolutions[5][0] = 1600; 
        Resolutions[5][1] = 900;
        Resolutions[6][0] = 1920; 
        Resolutions[6][1] = 1080;
        Resolutions[7][0] = 2048; 
        Resolutions[7][1] = 1152;
        Resolutions[8][0] = 2560; 
        Resolutions[8][1] = 1440;
        Resolutions[9][0] = 3200; 
        Resolutions[9][1] = 1800;
        Resolutions[10][0] = 3840; 
        Resolutions[10][1] = 2160;
        SetResolution(CurrentResolution);
    }
    public void MakeBigger()
    {
        if (CurrentResolution < 10)
        {
            CurrentResolution++;
            SetResolution(CurrentResolution);
        }
    }
    public void MakeSmaller()
    {
        if (CurrentResolution > 0)
        {
            CurrentResolution--;
            SetResolution(CurrentResolution);
        }
    }
    private void SetResolution(int Resolution)
    {
        ResolutionText.text = Resolutions[Resolution][0] + "X" + Resolutions[Resolution][1];
        Screen.SetResolution(Resolutions[Resolution][0], Resolutions[Resolution][1], FullScreen);
    }
}
