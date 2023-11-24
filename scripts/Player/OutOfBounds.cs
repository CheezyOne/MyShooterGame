using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public bool WentOutOfBounds = false;
    private void Update()
    { 
        if (transform.position.y < 0 && !WentOutOfBounds)
        {
            WentOutOfBounds = true;
            GetComponent<PlayerHP>().takeDamage(3000);
        }
    }
}
