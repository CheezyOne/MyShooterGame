using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private GameObject SniperRifle;
    [SerializeField] private GameObject DeathScreen;
    [SerializeField] private GameObject DeathButtons;
    [SerializeField] private GameObject HealthPoints;
    private CanvasGroup FadingScreen;
    public int PlayerHealth = 100;
    public bool isDead = false;
    private bool StopRotation = false;
    private float TimerForStopRotation = 2f;
    private float TimerForDead =1f;
    private float InvincibilityTimer = 0.05f;
    private bool Invincible = false;
    private Rigidbody RB;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        FadingScreen=DeathScreen.GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (PlayerHealth <= 0)
            PlayerHealth = 0;
        if (Invincible && InvincibilityTimer>0)
        {
            InvincibilityTimer -= Time.deltaTime;
        }
        else if(InvincibilityTimer<=0)
        {
            InvincibilityTimer = 0.05f;
            Invincible = false;
        }
        HealthPoints.GetComponent<TextMeshProUGUI>().text = "Çהמנמגüו:" + PlayerHealth;
        if (isDead)
        {
            FadingScreen.gameObject.SetActive(true);
            TimerForDead -= Time.deltaTime;
            if(TimerForDead<0 && FadingScreen.alpha<0.8f)
            {
                FadingScreen.alpha +=Time.deltaTime; 
            }
            if(GetComponent<OutOfBounds>().WentOutOfBounds)
            {
                FadingScreen.alpha += Time.deltaTime;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        if (StopRotation)
        {
            if (TimerForStopRotation > 0)
            {
                TimerForStopRotation -= Time.deltaTime;
            }
            else
            {
                RB.freezeRotation = true;
            }
        }
    }
    public void takeDamage(int damage)
    {
        if (!Invincible)
        {
            Invincible = true;
            PlayerHealth -= damage;
            if (PlayerHealth <= 0 && !isDead)
            {
                RB.isKinematic = false;
                isDead = true;
                Die();
            }
        }
    }
    private void Die()
    {
        RB.useGravity = true;
        RB.freezeRotation = false;
        StopRotation = true;
        RB.velocity = new Vector3
            (
            Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4)
            );
        if (SniperRifle.TryGetComponent<SniperRifle>(out SniperRifle SniperRifleScript))
        {
            if (SniperRifleScript.IsZoomed)
                SniperRifleScript.PlayerWithSniperRifleDied = true;
        }
        Destroy(GetComponent<PlayerMovement>());
        Destroy(GetComponent<PickUpLoot>());
        Destroy(transform.GetChild(0).GetComponent<MouseLook>());
        Destroy(GetComponent<CharacterController>());
        Cursor.lockState = CursorLockMode.None;
        DeathButtons.SetActive(true);
    }
}
