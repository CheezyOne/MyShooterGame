using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HealthHolder : MonoBehaviour
{
    public static Action onDeath;
    public int enemyHealth = 50;
    [SerializeField] private Transform HealthBar;
    private Camera PlayerCam;
    public bool isDead = false;
    [SerializeField] private Transform[] SparksPositions;
    [SerializeField] private ParticleSystem Sparks;
    private void Start()
    {
        PlayerCam = Camera.main; 
    }
    private void Update()
    {
        if (enemyHealth <= 0 && !isDead)
        {
            onDeath?.Invoke();
            Destroy(HealthBar.gameObject);
            isDead = true;
            Destroy(gameObject, 5f);
            foreach (Transform SparkPosition in SparksPositions)
            {
                Instantiate(Sparks , SparkPosition);
            }
        }
        try
        {
            HealthBar.rotation = Quaternion.LookRotation(HealthBar.position - PlayerCam.transform.position);
        }
        catch
        {

        }
    }
}
