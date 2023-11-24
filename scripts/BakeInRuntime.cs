using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BakeInRuntime : MonoBehaviour
{
    [SerializeField] private NavMeshSurface RebakingSurface;
    private void Start()
    {
        StartCoroutine(Rebake());
    }
    private IEnumerator Rebake()
    {
        RebakingSurface.BuildNavMesh();
        yield return new WaitForSeconds(1f);
        yield return Rebake();
    }
}
