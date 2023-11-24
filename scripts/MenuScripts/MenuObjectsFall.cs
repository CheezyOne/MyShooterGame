using UnityEngine;

public class MenuObjectsFall : MonoBehaviour
{
    [SerializeField] private GameObject[] AllObjects;
    [SerializeField] private Vector3[] AllFallingPositions;
    [SerializeField] private Vector3[] AllUpPositions;
    public static bool ShouldFall;
    void Start()
    {
        for(int i=0; i<AllObjects.Length; i++) 
        {
            AllUpPositions[i] = new Vector3(AllObjects[i].transform.position.x, AllObjects[i].transform.position.y, AllObjects[i].transform.position.z);
            AllFallingPositions[i]=new Vector3(AllObjects[i].transform.position.x, AllObjects[i].transform.position.y - 7f, AllObjects[i].transform.position.z);
        }
    }
    void Update()
    {
        if(ShouldFall)
        {
            for (int i =0;i<AllObjects.Length;i++)
            {
                AllObjects[i].transform.position = Vector3.MoveTowards(AllObjects[i].transform.position, AllFallingPositions[i] , Time.deltaTime*15f) ;
            }    
        }
        else
        {
            for (int i = 0; i < AllObjects.Length; i++)
            {
                AllObjects[i].transform.position = Vector3.MoveTowards(AllObjects[i].transform.position, AllUpPositions[i], Time.deltaTime * 15f);
            }
        }
    }
}
