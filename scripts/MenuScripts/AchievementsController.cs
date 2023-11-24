using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsController : MonoBehaviour
{
    private static string[] texts = new string[]
    {"ѕройти первый уровень, не убива€ дроида 495220",
        "",
        "",
        "",
        "",
        "",
        "",
    };
    private static bool[] isComplete = new bool[texts.Length];
    [SerializeField] private Sprite[] images = new Sprite[texts.Length];
    [SerializeField] private GameObject[] achivementImages = new GameObject[texts.Length];
    [SerializeField] private GameObject[] achivementTexts = new GameObject[texts.Length];
    private void OnEnable()
    {
        for (int i=0;i<texts.Length; i++)
        {
            if (isComplete[i])
            {
                achivementImages[i].transform.GetComponent<Image>().sprite = images[i];
                achivementTexts[i].transform.GetComponent<Text>().text = texts[i];
            }
        }
    }
}
