using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
public class EnemyPistolShooting : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private bool AddBulletSpread = true;
    public GameObject Head;
    private Vector3 BulletSpread = new Vector3(0.08f, 0.03f, 0.08f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private bool isSeeingPlayer = false;
    public GameObject Player;
    public bool ShouldTurnToPlayer = false;
    //public GameObject Player1;
    private float TimeBeforeNextShot = 1f;
    private float Range = 100f;
    private float AngleForVision = 90f;
    private float timeToEvade = 1.5f;
    [SerializeField] private LayerMask IgnoreItself;
    private EnemyNavMesh NavMesh;
    //public string[] Type;
    private void Start()
    {
        TimeBeforeNextShot = Random.Range(0.5f, 1.5f);
        TryGetComponent<EnemyNavMesh>(out NavMesh);
        GetComponent<EnemyPistolShooting>().enabled = false;
        GetComponent<EnemyPistolShooting>().enabled = true;
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
        timeToEvade -= Time.deltaTime;
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
            isSeeingPlayer = true;
            Head.transform.LookAt(Player.transform.position);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(Player.transform.position.x - Head.transform.position.x, 0f, Player.transform.position.z - Head.transform.position.z), 0.8f, 0.0f));
            NextShot();
        }
        else
        {
            isSeeingPlayer = false;
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
            TimeBeforeNextShot = Random.Range(0.5f, 1.5f);
            Shoot();
        }
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
    private void EvadeBullet()
    {
        if (timeToEvade > 0)
        {
            return;
        }
        else
        {
            if (isSeeingPlayer)
            {
                timeToEvade = 1.5f;
                float distanceToRunFor = Random.Range(1f, 7f);
                Vector3 positionToRunTo = transform.position + new Vector3
                    (Random.Range(-distanceToRunFor, distanceToRunFor), 0f, Random.Range(-distanceToRunFor, distanceToRunFor));
                NavMesh.positionToMoveTo = positionToRunTo;
                NavMesh.moveToPosition = true;
            }
        }
    }
    private void OnEnable()
    {
        Pistol.onShot += EvadeBullet;
        Shotgun.onShot += EvadeBullet;
        MachineGun.onShot += EvadeBullet;
        SniperRifle.onShot += EvadeBullet;
    }
    private void OnDisable()
    {
        Pistol.onShot -= EvadeBullet;
        Shotgun.onShot -= EvadeBullet;
        MachineGun.onShot -= EvadeBullet;
        SniperRifle.onShot -= EvadeBullet;
    }
}
