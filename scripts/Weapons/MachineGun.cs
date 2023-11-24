using System;
using TMPro;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public static Action onShot;
    [SerializeField] private GameObject Bullet;
    public Camera PlayerCam;
    [SerializeField] private bool AddBulletSpread = true;
    private Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private float Range = 10000f;
    private float SavedReloadTime = 2.5f;
    private float ReloadTime = 2.5f;
    private bool Reloading = false;
    private int Ammo = 30;
    [SerializeField] private LayerMask IgnorePlayer;
    [SerializeField] private LayerMask Enemies;
    private float TimeBeforeNextShot = 0.1f;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject AmmoText;
    private TextMeshProUGUI AmmoTextUI;
    private Animation Animations;
    private void Start()
    {
        Animations = GetComponent<Animation>();
        AmmoTextUI = AmmoText.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.layer == 3)
            {
                AmmoTextUI.text = "Патроны:" + Ammo;
                if (Ammo < 1)
                {
                    Animations.Play("AK-47Reload");
                    Reloading = true;
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
                        ReloadTime = SavedReloadTime;
                        Reloading = false;
                        Ammo = 30;
                    }
                }
            }
        }
        if (TimeBeforeNextShot <= 0)
        {
            if (Input.GetMouseButton(0) && !Player.GetComponent<PlayerHP>().isDead)
            {
                if (transform.parent != null)
                {
                    if (transform.parent.gameObject.layer == 3)
                    {
                        TimeBeforeNextShot = 0.1f;
                        onShot?.Invoke();
                        Ammo--;
                        Animations.Play("AK-47Shot");
                        Shoot();
                    }
                }
            }
        }
        else
        {
            TimeBeforeNextShot -= Time.deltaTime;
        }
    }
    private void Shoot()
    {
        Instantiate(ShootingParticle, BulletSpawnPoint);
        Vector3 BulletDirection = PlayerCam.transform.forward;
        Vector3 BulletSpawn = BulletSpawnPoint.position;
        BulletDirection = GetDirection();
        Physics.Raycast(PlayerCam.transform.position, BulletDirection, out RaycastHit hit, Range, IgnorePlayer);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawn, Quaternion.identity) as GameObject;
        BulletObj.GetComponent<Bullet>().hit = hit;
        BulletObj.GetComponent<Bullet>().BulletDirection = BulletDirection;
        BulletObj.GetComponent<Bullet>().Shooter = gameObject;
        BulletObj.GetComponent<Bullet>().StartPosition = PlayerCam.transform.position;
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * 15;
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
