using System.Collections;
using System.Threading;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask ShotObject;
    [SerializeField] private ParticleSystem ImpactParticle;
    public int BulletDamage = 10;
    public RaycastHit hit;
    public Vector3 StartPosition;
    public Vector3 BulletDirection;
    public GameObject Shooter;
    private int ShotObjectLayer=10;
    private int NormalLayer;
    private float TimerLayer = 0.005f;
    private void Start()
    {
        gameObject.layer = 9;
    }
    private void Update()
    {
        TimerLayer -= Time.deltaTime;
        if (TimerLayer <= 0)
            gameObject.layer = 6;
        Destroy(gameObject, 8f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Hit(collision);
        }
    }
    private bool CheckOfHittingItself(Collision collision)
    {
        try //Shooter maybe dead when hitting a target
        {
            if (collision.transform.IsChildOf(Shooter.transform))
            {
                return true;
            }
        }
        catch
        {

        }
        return false;
    }
    private void Hit(Collision collision)
    {
        if(CheckOfHittingItself(collision))
        {
            return;
        }

        NormalLayer = collision.gameObject.layer;
        collision.gameObject.layer = ShotObjectLayer;

        if (collision.transform.TryGetComponent<BodyPartHealth>(out BodyPartHealth EnemyHP))
        {
            if (Shooter == null)
            {
                EnemyHP.takeDamage(BulletDamage / 3);
            }
            else
            {
                // EnemyHP.gameObject.TryGetComponent<EnemyHB>(out EnemyHB EnemyHB);
                if (Shooter.transform.TryGetComponent<HealthHolder>(out HealthHolder ShooterHp))
                {
                    EnemyHP.takeDamage(BulletDamage / 3);
                }
                else
                {
                    EnemyHP.takeDamage(BulletDamage);
                }
            }
        }
        else if (collision.transform.TryGetComponent<PlayerHP>(out PlayerHP PlayerHP))
        {
            PlayerHP.takeDamage(BulletDamage);
        }
        Vector3 fromPosition = StartPosition;
        Vector3 toPosition = collision.contacts[0].point;
        Vector3 direction = toPosition - fromPosition;

        Physics.Raycast(StartPosition, direction, out RaycastHit hit, 10000f, ShotObject);
        if (!(hit.normal == Vector3.zero))
        {
            Instantiate(ImpactParticle, hit.point, Quaternion.LookRotation(hit.normal));
        }
        collision.gameObject.layer = NormalLayer;
        Destroy(gameObject);
    }
}
