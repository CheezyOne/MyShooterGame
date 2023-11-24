using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    private Animation animation;
    private void Awake()
    {
        animation=GetComponent<Animation>();
        ButtonManager.onLevelSelection += StarAnimation;
    }
    private void StarAnimation()
    {
        int AnimationNumber = Random.Range(1, 5);
        animation.Play("FadingInFromMenu"+ AnimationNumber);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene("Map");
    }
    private void OnDisable()
    {
        ButtonManager.onLevelSelection -= StarAnimation;
    }
}
