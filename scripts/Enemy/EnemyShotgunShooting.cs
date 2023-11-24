using System.Collections;
using System.Diagnostics.Contracts;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShotgunShooting : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private bool AddBulletSpread = true;
    public GameObject Head;
    private Vector3 BulletSpawnSpread = new Vector3(0f,0f, 0f);
    private Vector3 BulletSpread = new Vector3(0.007f, 0.015f, 0.007f);//(0.08f, 0.03f, 0.08f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    public GameObject Player;
    //public GameObject Player1;
    [SerializeField] private GameObject Weapon;
    private bool HasSeenPlayer = false;
    public bool ShouldTurnToPlayer = false;
    public Vector3 PositionToMoveTo;
    [SerializeField] private GameObject Feet;
    private EnemyNavMesh NavMesh;
    private float TimePassedSinceSeenPlayer=0f;
    private float TimeSeeingPlayer = 0f;
    private float AngleForVision = 90f;
    private float TimeBeforeNextShot;
    private float Range = 70f;
    [SerializeField] private LayerMask IgnoreItself;
    private void Start()
    {
        TimeBeforeNextShot = Random.Range(1f, 2f);
        NavMesh=Feet.GetComponent<EnemyNavMesh>();
        GetComponent<EnemyShotgunShooting>().enabled = false;
        GetComponent<EnemyShotgunShooting>().enabled = true;
        StartCoroutine(CheckVision());
    }
    IEnumerator CheckVision()
    {
        Vector3 DirectionToTarget = (Player.transform.position - transform.position).normalized;
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
        if (transform.GetComponent <HealthHolder>().isDead)
        {
            return;
        }
        if (Player.transform.parent.GetComponent<PlayerHP>().isDead)
        {
            Destroy(NavMesh);
            return;
        }
        Physics.Raycast(Head.transform.position, Player.transform.position - Head.transform.position, out RaycastHit hit, Range, IgnoreItself);
        if (hit.transform == null)
        {
            return;
        }
        if (hit.collider.name == "Player")
        {
            //Head.transform.LookAt(Player.transform.position);
            TimeSeeingPlayer += Time.deltaTime;
            if(TimeSeeingPlayer>3f)
            {
                NavMesh.positionToMoveTo=hit.transform.position;
                NavMesh.moveToPosition = true;
                TimeSeeingPlayer = 0f;
            }
            TimePassedSinceSeenPlayer = 0f;
            HasSeenPlayer = true;
            PositionToMoveTo = hit.transform.position;
            Head.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(Head.transform.forward, Player.transform.position - Head.transform.position, 0.08f, 0.0f));
            NextShot();
        }
        else
        {
            TimeSeeingPlayer = 0f;
            if (HasSeenPlayer)
            {
                TimePassedSinceSeenPlayer += Time.deltaTime;
            }
            if(TimePassedSinceSeenPlayer>1f)
            {
                TimePassedSinceSeenPlayer = 0;
                NavMesh.positionToMoveTo = PositionToMoveTo;
                NavMesh.moveToPosition = true;
                HasSeenPlayer = false;
            }
        }
    }

    private void NextShot()
    {
        if(TimeBeforeNextShot>0)
        {
            TimeBeforeNextShot -= Time.deltaTime;
        }
        else
        {
            Vector3 DirectionToTarget = (Player.transform.position - transform.position).normalized;
            if (!(Vector3.Angle(Head.transform.forward, DirectionToTarget) < AngleForVision / 4))
            {
                return;
            }
            TimeBeforeNextShot = Random.Range(1f,2f);
            Instantiate(ShootingParticle, BulletSpawnPoint);
            for (int i = 0; i < 6; i++)
            {
                Shoot();
            }
        }
    }
 
    private void Shoot()
    {
        Vector3 BulletSpawn = BulletSpawnPoint.position;
        BulletSpawn = SpawnBullet();
        Vector3 BulletDirection = Head.transform.forward;
        BulletDirection = GetDirection();
        Physics.Raycast(Head.transform.position, BulletDirection, out RaycastHit hit, IgnoreItself);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity) as GameObject;
        Bullet BulletComponent = BulletObj.GetComponent<Bullet>();
        BulletComponent.hit = hit;
        BulletComponent.StartPosition = Head.transform.position;
        BulletComponent.BulletDirection = BulletDirection;
        BulletComponent.Shooter = gameObject;
        float BulletSpeed = Random.Range(20, 40);
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * BulletSpeed;
    }
    private Vector3 SpawnBullet()
    {
        Vector3 BulletSpawn = BulletSpawnPoint.position;
        BulletSpawn += new Vector3
           (
                Random.Range(-BulletSpawnSpread.x, BulletSpawnSpread.x),
                Random.Range(-BulletSpawnSpread.y, BulletSpawnSpread.y),
                0f
           );

        return BulletSpawn;
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
