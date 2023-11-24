using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnemy : MonoBehaviour
{
    [SerializeField] private int Speed;
    [SerializeField] private bool[] EnemyType = { false, false, false, false };
    [SerializeField] private GameObject Bullet;
    public GameObject Head;
    private Vector3 BulletSpawnSpread = new Vector3(0.1f, 0.1f, 0f);
    private Vector3 BulletSpread = new Vector3(0.007f, 0.015f, 0.007f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private bool Stop=false;
    private bool TargetIsDead=false;
    public GameObject Target;
    public GameObject Feet;
    private float TimeBeforeNextShot;
    [SerializeField] private LayerMask IgnoreItself;
    private void Start()
    {
        Destroy(gameObject, 20f);
        TimeBeforeNextShot = Random.Range(1f, 2f);
    }
    private void Update()
    {
        if (Stop)
            return;
        if (transform.GetComponent<HealthHolder>().isDead)
        {
            Stop = true;
            return;
        }
        if(TargetIsDead || Target == gameObject)
        {
            Target = transform.parent.transform.GetChild(Random.Range(0, transform.parent.transform.childCount)).gameObject;
            TargetIsDead = false;
            return;
        }
        if(Target==null)
        {
            TargetIsDead = true;
            return;
        }
        if (Target.GetComponent<HealthHolder>().isDead)
        {
            TargetIsDead = true;
            return;
        }
        Head.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(Head.transform.forward, Target.transform.position - Head.transform.position, 0.08f, 0.0f));

            NextShot();
    }

    private void NextShot()
    {
        if (TimeBeforeNextShot > 0)
        {
            TimeBeforeNextShot -= Time.deltaTime;
        }
        else
        {
            if (EnemyType[0] == true)
            {
                TimeBeforeNextShot = Random.Range(1f, 2f);
                Instantiate(ShootingParticle, BulletSpawnPoint);
                for (int i = 0; i < 6; i++)
                {
                    Shoot();
                }
            }
            else if(EnemyType[1] == true)
            {
                TimeBeforeNextShot = Random.Range(0.5f, 1f);
                Instantiate(ShootingParticle, BulletSpawnPoint);
                Shoot();
            }
            else if (EnemyType[2] == true)
            {
                TimeBeforeNextShot = Random.Range(2f, 3f);
                Instantiate(ShootingParticle, BulletSpawnPoint);
                Shoot();
            }
            else if (EnemyType[3] == true)
            {
                TimeBeforeNextShot = Random.Range(1f, 2f);
                Instantiate(ShootingParticle, BulletSpawnPoint);
                StartCoroutine(RifleShooting());
            }
        }
    }
    private IEnumerator RifleShooting()
    {
        for (int i = 0; i < 5; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.06f);
        }
        StopCoroutine(RifleShooting());
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
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * (Speed+Random.Range(-3f,3f));
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
