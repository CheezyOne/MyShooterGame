using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action onDeath;
    [SerializeField] private Image RemainingHealth;
    [SerializeField] private Transform HealthBar;
    //private Transform HealtBarTransform;
    private Camera PlayerCam;
    private float MaxHealth;
    public int enemyHealth = 50;
    public bool isDead = false;
    private void Start()
    {
        PlayerCam = Camera.main;
        MaxHealth = enemyHealth;
    }
    private void Update()
    {
        try
        {
            HealthBar.rotation = Quaternion.LookRotation(HealthBar.position - PlayerCam.transform.position);
        }
        catch
        {

        }
    }
    public void takeDamage(int damage)
    {
        enemyHealth -= damage;
        try
        {
            RemainingHealth.fillAmount = enemyHealth / MaxHealth;
        }
        catch 
        { }
        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    private void Die()
    {
        onDeath?.Invoke();
        Destroy(transform.GetComponent<EnemyNavMesh>());
        Destroy(transform.GetComponent<NavMeshAgent>());
        transform.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddForce(-transform.forward * 200f);
        Destroy(gameObject,5f);
    }
}
