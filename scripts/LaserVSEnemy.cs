using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVSEnemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<BodyPartHealth>().takeDamage(25);
    }
}
