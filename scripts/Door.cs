using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool SpecialCondition = false;
    [SerializeField] private Transform DoorPlatform;
    private float OpenDistance = 4.55f;
    private float OpenPosition = 0f;
    public bool Open=false;
    private void OnEnable()
    {
        LevelObjective.onObjetiveCompletion += OpenDoor;
    }
    private void OnDisable()
    {
        LevelObjective.onObjetiveCompletion -= OpenDoor;
    }
    void Update()
    {
        if (Open)
        DoorPlatform.position = Vector3.Lerp(DoorPlatform.position, new Vector3(DoorPlatform.position.x, OpenPosition, DoorPlatform.position.z), Time.deltaTime);
    }
    private void OpenDoor()
    {
        if (SpecialCondition)
            return;
        Open = true;
        OpenPosition = DoorPlatform.position.y + OpenDistance;
    }
}
