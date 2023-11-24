using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public static Action onShot;
    [SerializeField] private GameObject Bullet;
    public Camera PlayerCam;
    [SerializeField] private bool AddBulletSpread = true;
    private Vector3 BulletSpawnSpread = new Vector3(0.1f, 0.1f, 0f);
    private Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private float Range = 10000f;
    private float ReloadTime = 6.5f;
    private int Ammo = 8;
    private bool Reloading = false;
    [SerializeField] private LayerMask IgnorePlayer;
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject AmmoText;
    private TextMeshProUGUI AmmoTextUI;
    private float NextShotTime = 0.3f;
    private bool WaitForNextShot = false;
    private Animation Animations;
    private void Start()
    {
        AmmoTextUI = AmmoText.GetComponent<TextMeshProUGUI>();
        Animations = GetComponent<Animation>();
    }
    private void Update()
    {
        if (WaitForNextShot)
        {
            AmmoTextUI.text = "Патроны:" + Ammo;
            NextShotTime -= Time.deltaTime;
            if (NextShotTime < 0)
            {
                WaitForNextShot = false;
                NextShotTime = 0.3f;
            }
            return;
        }
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.layer == 3)
            {
                AmmoTextUI.text = "Патроны:" + Ammo;
                if (Ammo < 1)
                {
                    Reloading = true;
                    Animations.Play("ShotgunReload");
                }
                if (Reloading)
                {
                    ReloadTime -= Time.deltaTime;
                    if (ReloadTime > 0)
                    {
                        return;
                    }
                    else
                    {
                        ReloadTime = 6.5f;
                        Reloading = false;
                        Ammo = 8;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && !Player.GetComponent<PlayerHP>().isDead)
        {
            if(transform.parent!=null)
            {
                if(transform.parent.gameObject.layer == 3)
                {
                    WaitForNextShot = true;
                    Animations.Play("ShotgunShot");
                    Instantiate(ShootingParticle, BulletSpawnPoint);
                    onShot?.Invoke();
                    Ammo--;
                    for (int i = 0; i < 6; i++)
                    {
                        Shoot();
                    }
                }
            }
        }
    }
    private void Shoot()
    {
        Vector3 BulletSpawn = SpawnBullet();
        Vector3 BulletDirection = PlayerCam.transform.forward;
        BulletDirection = GetDirection();
        Physics.Raycast(PlayerCam.transform.position, BulletDirection, out RaycastHit hit, Range, IgnorePlayer);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawn, Quaternion.identity) as GameObject;
        BulletObj.GetComponent<Bullet>().hit = hit;
        BulletObj.GetComponent<Bullet>().BulletDirection = BulletDirection;
        BulletObj.GetComponent<Bullet>().StartPosition = PlayerCam.transform.position;
        BulletObj.GetComponent<Bullet>().Shooter = gameObject;
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawn).normalized * 15;

        
    }
    private Vector3 SpawnBullet()
    {
        Vector3 BulletSpawn = BulletSpawnPoint.position;
        BulletSpawn += new Vector3
           (
                UnityEngine.Random.Range(-BulletSpawnSpread.x, BulletSpawnSpread.x),
                UnityEngine.Random.Range(-BulletSpawnSpread.y, BulletSpawnSpread.y),
                0f
           );

        return BulletSpawn;
    }
    private Vector3 GetDirection()
    {
        Vector3 BulletDirection = PlayerCam.transform.forward;
        if (AddBulletSpread)
        {
            BulletDirection += new Vector3
                (
                UnityEngine.Random.Range(-BulletSpread.x, BulletSpread.x),
                UnityEngine.Random.Range(-BulletSpread.y, BulletSpread.y),
                UnityEngine.Random.Range(-BulletSpread.z, BulletSpread.z)
                );
            BulletDirection.Normalize();
        }
        return BulletDirection;
    }
}
