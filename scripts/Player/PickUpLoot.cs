using Unity.VisualScripting;
using UnityEngine;
using System;

public class PickUpLoot : MonoBehaviour
{
    public Camera playerCam;
    private RaycastHit hit;
    private Transform loot;
    private float pickUpRange = 10f;
    private bool isPickedUp=false;
    private float size1, size2, size3;
    private bool ShouldScaleUp = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPickedUp)
            {
                if (hit.point != null && hit.transform.GetComponent<Loot>() != null)
                {
                    isPickedUp = false;
                    loot.localScale = new Vector3(size1,size2,size3);
                    loot.transform.localPosition = new Vector3(-0.25f, -0.131f, 1f);
                    loot.parent = null;
                    loot.AddComponent<Rigidbody>();
                    loot.GetComponent<Collider>().enabled = true;
                    loot.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward*300f);
                }
            }
            else
            {
                Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, pickUpRange);
                if (hit.transform != null)
                {
                    if (hit.transform.GetComponent<Loot>() != null)
                    {
                        if (hit.transform.GetComponent<Loot>().ShouldBeScaled)
                            ShouldScaleUp = true;
                        loot = hit.transform;
                        isPickedUp = true;
                        if (ShouldScaleUp)
                        {
                            size1 = loot.localScale.x;
                            size2 = loot.localScale.y;
                            size3 = loot.localScale.z;
                        }
                        else
                        {
                            size1 = 1f;
                            size2 = 1f;
                            size3 = 1f;
                        }
                        loot.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        loot.SetParent(playerCam.transform);
                        loot.transform.localPosition = new Vector3(-0.25f, -0.131f, 0.267f);
                        Destroy(loot.GetComponent<Rigidbody>());
                        loot.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }
    }
}
