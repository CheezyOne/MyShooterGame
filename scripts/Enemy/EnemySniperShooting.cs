using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySniperShooting : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    public GameObject Head;
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    public GameObject Player;
    //public GameObject Weapon;
    //public GameObject Player1;
    public bool ShouldTurnToPlayer = false;
    private float TimeBeforeNextShot = 4f;
    private float Range = 100f;
    private float AngleForVision = 90f;
    private EnemyNavMesh NavMesh;
    [SerializeField] private LayerMask IgnoreItself;
    //public string[] Type;
    private void Start()
    {
        TimeBeforeNextShot = Random.Range(2f, 4f);
        TryGetComponent<EnemyNavMesh>(out NavMesh);
        GetComponent<EnemySniperShooting>().enabled = false;
        GetComponent<EnemySniperShooting>().enabled = true;
        StartCoroutine(CheckVision());
    }
    IEnumerator CheckVision()
    {
        Vector3 DirectionToTarget = (Player.transform.position - transform.position).normalized;
        //Debug.DrawLine(Head.transform.position, Player.transform.position - Head.transform.position, Color.red);
        if (Vector3.Angle(transform.forward, DirectionToTarget) < AngleForVision / 2)
        {
            ShouldTurnToPlayer = true;
            StopCoroutine(CheckVision());
        }
        yield return new WaitForSeconds(0.3f);
        yield return CheckVision();
    }
    private void Update()
    {
        if (!ShouldTurnToPlayer)
        {
            return;
        }
        if (transform.GetComponent<EnemyHealth>().isDead)
        {
            return;
        }
        if (Player.transform.parent.GetComponent<PlayerHP>().isDead)
        {
            Destroy(GetComponent<EnemyNavMesh>());
            return;
        }
        Physics.Raycast(Head.transform.position, Player.transform.position - Head.transform.position, out RaycastHit hit, Range, IgnoreItself);
        if (hit.transform == null)
        {
            return;
        }

        if (hit.collider.name == "Player")
        {
            //NavMesh.positionToMoveTo = hit.transform.position;
            Head.transform.LookAt(Player.transform.position);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,new Vector3( Player.transform.position.x - Head.transform.position.x,0f, Player.transform.position.z - Head.transform.position.z), 0.8f, 0.0f));
            NextShot();
        }
    }

    private void NextShot()
    {
        if (TimeBeforeNextShot > 0)
        {
            TimeBeforeNextShot -= Time.deltaTime;
        }
        else
        {
            TimeBeforeNextShot = Random.Range(2f, 4f);
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(ShootingParticle, BulletSpawnPoint);
        Vector3 BulletDirection = Head.transform.forward;
        Physics.Raycast(Head.transform.position, BulletDirection, out RaycastHit hit, IgnoreItself);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity) as GameObject;
        BulletObj.GetComponent<Bullet>().hit = hit;
        BulletObj.GetComponent<Bullet>().StartPosition = Head.transform.position;
        BulletObj.GetComponent<Bullet>().BulletDirection = BulletDirection;
        BulletObj.GetComponent<Bullet>().Shooter = gameObject;
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * 70;
    }
}
