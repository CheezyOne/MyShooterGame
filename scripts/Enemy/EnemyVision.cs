using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public bool StartTurning = false;
    private EnemyPistolShooting enemyPistolShooting;
    private EnemyRifleShooting enemyRifleShooting;
    private EnemyShotgunShooting enemyShotgunShooting;
    private EnemySniperShooting enemySniperShooting;
    private float DistanceOfHearing = 20f;
    private bool HasPistol, HasRifle, HasSniper, HasShotgun;
    [SerializeField] private Transform Player;
    private void Start()
    {
        if(TryGetComponent<EnemyPistolShooting>(out enemyPistolShooting))
        {
            HasPistol = true;
            //enemyPistolShooting.ShouldTurnToPlayer = true;
        }
        if(TryGetComponent<EnemyRifleShooting>(out enemyRifleShooting))
        {
            HasRifle = true;
            //enemyRifleShooting.ShouldTurnToPlayer = true;
        }
        if(gameObject.TryGetComponent<EnemyShotgunShooting>(out enemyShotgunShooting))
        {
            HasShotgun = true;
        }
        if(TryGetComponent<EnemySniperShooting>(out enemySniperShooting))
        {
            HasSniper = true;
            //enemySniperShooting.ShouldTurnToPlayer = true;
        }
    }
    private void Update()
    {
        if (HasPistol)
        {
            if(StartTurning)
            {
                enemyPistolShooting.ShouldTurnToPlayer = true;
            }
        }
        else if(HasRifle)
        {
            if (StartTurning)
            {
                enemyRifleShooting.ShouldTurnToPlayer = true;
            }
        }
        else if (HasShotgun)
        {
            if (StartTurning)
            {
                enemyShotgunShooting.ShouldTurnToPlayer = true;
            }
        }
        else if(HasSniper)
        {
            if (StartTurning)
            {
                enemySniperShooting.ShouldTurnToPlayer = true;
            }
        }
    }
    private void HeardAShot()
    {
        if( Vector3.Distance(gameObject.transform.position, Player.position)< DistanceOfHearing)
            StartTurning = true;
    }
    private void OnEnable()
    {
        MachineGun.onShot += HeardAShot;
        Pistol.onShot += HeardAShot;
        Shotgun.onShot += HeardAShot;
        SniperRifle.onShot += HeardAShot;
    }
    private void OnDisable()
    {
        MachineGun.onShot -= HeardAShot;
        Pistol.onShot -= HeardAShot;
        Shotgun.onShot -= HeardAShot;
        SniperRifle.onShot -= HeardAShot;
    }
}
