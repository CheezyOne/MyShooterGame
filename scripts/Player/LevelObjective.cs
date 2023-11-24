using System;
using TMPro;
using UnityEngine;


public class LevelObjective : MonoBehaviour
{
    public static Action onObjetiveCompletion;
    public int KillCounter = 0, SpecialCounter = 0, LootCounter = 0;
    public float Timer = 0;
    private int KC = 0, SC = 0, LC = 0;
    private float TimeCounter = 0;
    public string SpecialObjective;
    public int CountTo = 0;
    private bool[] IsShowing = { false, false, false, false };
    public bool[] LevelObjectives = { false, false, false, false };//kills,special, lott,time
    [SerializeField] private TextMeshProUGUI ObjectiveText;
    [SerializeField] private TextMeshProUGUI[] Objectives;


    private void KillsObjectiveProgression()
    {
        ObjectiveText.text = ObjectiveText.text.Replace("\n Убить всех противников: " + KC + "/" + KillCounter,"");
        KC++;
        IsShowing[0] = false;
        if(KC>=KillCounter)
        {
            ObjectiveComplete();
        }
    }
    private void OnEnable()
    {
        HealthHolder.onDeath += KillsObjectiveProgression;
    }
    private void OnDisable()
    {
        HealthHolder.onDeath -= KillsObjectiveProgression;
    }
    private void Update()
    {
        if (KillCounter != 0 && !IsShowing[0])
        {
            IsShowing[0] = true;
            ObjectiveText.text += "\n Убить всех противников: " + KC + "/" + KillCounter;
        }
        if (SpecialCounter != 0 && !IsShowing[1])
        {
            IsShowing[1] = true;
            ObjectiveText.text += "\n" + SpecialObjective + " " + SC + "/" + SpecialCounter;
        }
        if (LootCounter != 0 && !IsShowing[2])
        {
            IsShowing[2] = true;
            ObjectiveText.text += "\n Принести объекты на базу: " + LC + "/" + LootCounter;
        }
        if (Timer != 0 && !IsShowing[3])
        {
            IsShowing[3] = true;
            ObjectiveText.text += "\n Времени осталось: " + TimeCounter + "/" + Timer;
        }
    }
    private void ObjectiveComplete()
    {
        onObjetiveCompletion?.Invoke();
    }
}
