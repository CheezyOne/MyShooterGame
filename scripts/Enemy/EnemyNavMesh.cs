using UnityEngine;
using UnityEngine.AI;
public class EnemyNavMesh : MonoBehaviour
{
    public Vector3 positionToMoveTo;
    public bool moveToPosition=false;
    public GameObject Enemy;
    private NavMeshAgent agent;
    
    private void Awake()
    {
        agent=Enemy.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (moveToPosition)
        {
            agent.destination = positionToMoveTo;
        }
    }
}
