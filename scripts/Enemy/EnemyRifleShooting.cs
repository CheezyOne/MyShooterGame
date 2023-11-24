using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRifleShooting : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private bool AddBulletSpread = true;
    public GameObject Head;
    private Vector3 BulletSpread = new Vector3(0.11f, 0.05f, 0.11f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    public GameObject Player;
    public bool ShouldTurnToPlayer = false;
    //public GameObject Player1;
    private float TimeBeforeNextShot = 2f;
    private float Range = 100f;
    private float TimeSeeingPlayer = 0f;
    private float AngleForVision = 90f;
    private float MoveToRandomPosition = 5f;
    private EnemyNavMesh NavMesh;

    [SerializeField] private LayerMask IgnoreItself;
    //public string[] Type;
    private void Start()
    {
        TimeBeforeNextShot = Random.Range(0.5f, 2.5f);
        TryGetComponent<EnemyNavMesh>(out NavMesh);
        GetComponent<EnemyRifleShooting>().enabled = false;
        GetComponent<EnemyRifleShooting>().enabled = true;
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
            TimeSeeingPlayer += Time.deltaTime;
            if (TimeSeeingPlayer > 2f)
            {
                NavMesh.positionToMoveTo = transform.position + new Vector3
                (
                Random.Range(-MoveToRandomPosition, MoveToRandomPosition),
                transform.position.y,
                Random.Range(-MoveToRandomPosition, MoveToRandomPosition)
                );
                TimeSeeingPlayer = 0f;
                NavMesh.moveToPosition = true;
            }
                Head.transform.LookAt(Player.transform.position);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(Player.transform.position.x - Head.transform.position.x, 0f, Player.transform.position.z - Head.transform.position.z), 0.8f, 0.0f));
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
            GetComponent<EnemyRifleShooting>().enabled = false;
            GetComponent<EnemyRifleShooting>().enabled = true;
            TimeBeforeNextShot = Random.Range(0.5f, 2.5f);
            StartCoroutine(Shooting());
        }
    }
    private IEnumerator Shooting()
    {
        for (int i = 0; i < 6; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.08f);
        }
        StopCoroutine(Shooting());
    }
    private void Shoot()
    {
        Instantiate(ShootingParticle, BulletSpawnPoint);
        Vector3 BulletDirection = Head.transform.forward;
        BulletDirection = GetDirection();
        Physics.Raycast(Head.transform.position, BulletDirection, out RaycastHit hit, IgnoreItself);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity) as GameObject;
        BulletObj.GetComponent<Bullet>().hit = hit;
        BulletObj.GetComponent<Bullet>().StartPosition = Head.transform.position;
        BulletObj.GetComponent<Bullet>().BulletDirection = BulletDirection;
        BulletObj.GetComponent<Bullet>().Shooter = gameObject;
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * 30;
    }

    private Vector3 GetDirection()
    {
        Vector3 BulletDirection = Head.transform.forward;
        if (AddBulletSpread)
        {
            BulletDirection += new Vector3
                (
                Random.Range(-BulletSpread.x, BulletSpread.x),
                Random.Range(-BulletSpread.y, BulletSpread.y),
                Random.Range(-BulletSpread.z, BulletSpread.z)
                );
            BulletDirection.Normalize();
        }
        return BulletDirection;
    }
}
