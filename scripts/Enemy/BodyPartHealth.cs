using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BodyPartHealth : MonoBehaviour
{
    [SerializeField] private Image RemainingHealth;

    [SerializeField] private GameObject Enemy;
    private HealthHolder EnemyHealth;
    private float TimeBeforeDisappearence = 0f;
    //private Transform HealtBarTransform;

    private float MaxHealth;
    public bool isDead = false;
    private void Start()
    {
        EnemyHealth=Enemy.GetComponent<HealthHolder>();
        MaxHealth = EnemyHealth.enemyHealth;
    }
    private void Update()
    {
        if (EnemyHealth.enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    public void takeDamage(int damage)
    {
        EnemyHealth.enemyHealth -= damage;
        try
        {
            RemainingHealth.fillAmount = EnemyHealth.enemyHealth / MaxHealth;
        }
        catch
        { 

        }
        if (Enemy != null)
        {
            Enemy.GetComponent<HealthHolder>().enemyHealth = EnemyHealth.enemyHealth;
        }
    }
    private void Die()
    {
        transform.SetParent(null);
        Destroy(transform.GetComponent<EnemyNavMesh>());
        Destroy(transform.GetComponent<NavMeshAgent>());
        transform.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddForce(-transform.forward*10);
        TimeBeforeDisappearence = UnityEngine.Random.Range(3f, 5f);
        Destroy(gameObject, TimeBeforeDisappearence);
    }
}
