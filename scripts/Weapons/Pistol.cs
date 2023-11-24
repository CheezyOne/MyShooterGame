using System;
using TMPro;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public static Action onShot;
    [SerializeField] private GameObject Bullet;
    public Camera PlayerCam;
    [SerializeField] private bool AddBulletSpread = true;
    private Vector3 BulletSpread = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private float Range = 10000f;
    private float SavedReloadTime = 2f;
    private float ReloadTime = 2f;
    private bool Reloading = false;
    private int Ammo = 12;
    [SerializeField] private LayerMask IgnorePlayer;
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject AmmoText;
    private float NextShotTime = 0.05f;
    private bool WaitForNextShot = false;
    private Animation Animations;
    private TextMeshProUGUI AmmoTextUI;
    [SerializeField] private GameObject PauseCanvas;
    private void Start()
    {
        AmmoTextUI = AmmoText.GetComponent<TextMeshProUGUI>();
        Animations = GetComponent<Animation>();
    }
    void Update()
    {
        if (PauseCanvas.activeSelf)
        {
            return; //Copy that part
        }
        if (WaitForNextShot)
        {
            NextShotTime -= Time.deltaTime;
            if (NextShotTime < 0)
            {
                WaitForNextShot = false;
                NextShotTime = 0.05f;
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
                    Animations.Play("PistolReload");
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
                        Ammo = 12;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && !Player.GetComponent<PlayerHP>().isDead)
        {
            if (transform.parent != null)
            {
                if (transform.parent.gameObject.layer == 3)
                {
                    if (!CheckTargetDistance()) //This part should be copied to other weapons
                        return;
                    Animations.Play("PistolShot");
                    onShot?.Invoke();
                    Ammo--;
                    Shoot();
                }
            }
        }
    }
    private bool CheckTargetDistance()
    {
        Vector3 BulletDirection = PlayerCam.transform.forward;
        BulletDirection = GetDirection();
        Physics.Raycast(PlayerCam.transform.position, BulletDirection, out RaycastHit hit, Range, IgnorePlayer);
        if (hit.distance <= 1.5f)
            return false;
        return true;
    }
    private void Shoot()
    {
        Instantiate(ShootingParticle, BulletSpawnPoint);
        Vector3 BulletDirection = PlayerCam.transform.forward;
        BulletDirection = GetDirection();
        Physics.Raycast(PlayerCam.transform.position, BulletDirection, out RaycastHit hit, Range, IgnorePlayer);
        if (hit.distance <= 1.5f)
            return;
        GameObject BulletObj = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity) as GameObject;
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
