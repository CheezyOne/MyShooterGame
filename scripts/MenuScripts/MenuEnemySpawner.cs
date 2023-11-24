using System.Collections;
using UnityEngine;

public class MenuEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private Transform[] Spawns;
    public bool isActive = false;
    private GameObject FirstEnemy, SecondEnemy;
    private int FirstEnemyNumber, SecondEnemyNumber;
    private Vector3 FirstEnemyPosition, SecondEnemyPosition;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    private IEnumerator SpawnEnemies()
    {
        if(!isActive)
        {
            yield return new WaitForSeconds(2.5f);
            yield return SpawnEnemies();
        }
        FirstEnemyNumber = Random.Range(0, 3);
        SecondEnemyNumber = Random.Range(0, 3);
        FirstEnemyPosition = new Vector3(Spawns[0].position.x, Spawns[0].position.y, Spawns[0].position.z+Random.Range(-5f,5f));
        SecondEnemyPosition = new Vector3(Spawns[1].position.x, Spawns[1].position.y, Spawns[1].position.z + Random.Range(-5f, 5f));
        FirstEnemy=Instantiate(Enemies[FirstEnemyNumber], FirstEnemyPosition, Quaternion.identity);
        SecondEnemy = Instantiate(Enemies[SecondEnemyNumber], SecondEnemyPosition, Quaternion.identity);
        FirstEnemy.GetComponent<MenuEnemy>().Target = SecondEnemy;
        SecondEnemy.GetComponent<MenuEnemy>().Target = FirstEnemy;
        FirstEnemy.GetComponent<MenuEnemy>().Feet.GetComponent<EnemyNavMesh>().positionToMoveTo = Spawns[1].position;
        FirstEnemy.GetComponent<MenuEnemy>().Feet.GetComponent<EnemyNavMesh>().moveToPosition=true;
        SecondEnemy.GetComponent<MenuEnemy>().Feet.GetComponent<EnemyNavMesh>().positionToMoveTo = Spawns[0].position;
        SecondEnemy.GetComponent<MenuEnemy>().Feet.GetComponent<EnemyNavMesh>().moveToPosition = true;
        FirstEnemy.transform.SetParent(gameObject.transform);
        SecondEnemy.transform.SetParent(gameObject.transform);
        yield return new WaitForSeconds(2.5f);
        yield return SpawnEnemies();
    }
}
