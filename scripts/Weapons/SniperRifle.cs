using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SniperRifle : MonoBehaviour
{
    public static Action onShot;
    [SerializeField] private GameObject Bullet;
    public Camera PlayerCam;
    [SerializeField] private ParticleSystem ShootingParticle;
    [SerializeField] private Transform BulletSpawnPoint;
    private float Range = 10000f;
    private float ReloadTime = 6.5f;
    private int Ammo = 5;
    private bool Reloading = false;
    [SerializeField] private LayerMask IgnorePlayer;
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private Image NormalPointer;
    [SerializeField] private GameObject ZoomImage;
    [SerializeField] private Transform SpaceForScope;
    [SerializeField] private Transform NormalPosition;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerUI;
    public bool IsZoomed = false;
    public bool PlayerWithSniperRifleDied=false;

    private float NextShotTime = 0.6f;
    private bool WaitForNextShot = false;

    [SerializeField] private GameObject AmmoText;
    private TextMeshProUGUI AmmoTextUI;
    private Animation Animations;
    private PlayerHP PlayerHealth;
    private void Start()
    {
        AmmoTextUI = AmmoText.GetComponent<TextMeshProUGUI>();
        Animations = GetComponent<Animation>();
        PlayerHealth = Player.GetComponent<PlayerHP>();
    }
    void Update()
    {
        if(PlayerHealth.isDead)
        {
            Animations.Stop();
            return;
        }
        if (WaitForNextShot)
        {
            AmmoTextUI.text = "Патроны:" + Ammo;
            NextShotTime -= Time.deltaTime;
            if (NextShotTime < 0)
            {
                WaitForNextShot = false;
                NextShotTime = 0.6f;
            }
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (transform.parent != null)
            {
                if (transform.parent.gameObject.layer == 3)
                {
                    if (Reloading)
                        return;
                    Zoom();
                }
            }
        }
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.layer == 3)
            {
                AmmoTextUI.text = "Патроны:" + Ammo;
                if (Ammo < 1)
                {
                    Reloading = true;
                    Animations.Play("SniperReload");
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
                        Ammo = 5;
                    }
                }
            }
        }
        if (PlayerWithSniperRifleDied)
        {
            Zoom();
            PlayerWithSniperRifleDied = false;
        }    
        if (Input.GetMouseButtonDown(0) && !PlayerHealth.isDead)
        {
            if (transform.parent != null)
            {
                if (transform.parent.gameObject.layer == 3)
                {
                    WaitForNextShot = true;
                    if (IsZoomed)
                        Zoom();
                    Animations.Play("SniperShot");
                    Instantiate(ShootingParticle, BulletSpawnPoint);
                    onShot?.Invoke();
                    Ammo--;
                    Shoot();
                }
            }
        }
    }
    private void Shoot()
    {
        Instantiate(ShootingParticle, BulletSpawnPoint);
        Vector3 BulletDirection = PlayerCam.transform.forward;
        Physics.Raycast(PlayerCam.transform.position, BulletDirection, out RaycastHit hit, Range, IgnorePlayer);
        GameObject BulletObj = Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity) as GameObject;
        BulletObj.GetComponent<Bullet>().hit = hit;
        BulletObj.GetComponent<Bullet>().BulletDirection = BulletDirection;
        BulletObj.GetComponent<Bullet>().Shooter = gameObject;
        BulletObj.GetComponent<Bullet>().StartPosition = PlayerCam.transform.position;
        BulletObj.GetComponent<Rigidbody>().velocity = (hit.point - BulletSpawnPoint.position).normalized * 50f;
    }
    public void Zoom()
    {
        if (!IsZoomed)
        {
            gameObject.transform.position = SpaceForScope.position;
            ZoomImage.SetActive(true);
            PlayerCam.fieldOfView = 10f;
            IsZoomed = true;
            PlayerUI.SetActive(false);
        }
        else
        {
            gameObject.transform.position = NormalPosition.position;
            ZoomImage.SetActive(false);
            PlayerCam.fieldOfView = 60f;
            IsZoomed = false;
            PlayerUI.SetActive(true);
        }
    }
}
