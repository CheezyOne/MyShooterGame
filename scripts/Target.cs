using UnityEngine;

public class Target : MonoBehaviour
{
    public float propHealth = 50f;
    
    public void takeDamage(float damage)
    {
        propHealth -= damage;
        if(propHealth <= 0)
        {
            Die();
        }
    }    
    private void Die()
    {
        Destroy(gameObject);
    }
}
