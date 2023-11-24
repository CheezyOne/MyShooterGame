using UnityEngine;
using UnityEngine.UIElements;

public class GivePlayerClass : MonoBehaviour
{
    public GameObject[] Weapons = new GameObject[3];
    public GameObject Player;
    public Transform PlaceForGun;
    void Start()
    {
        switch (DataHolder.ChosenCharacter)
        {
            case "Pistolboi":
                {
                    GiveWeapon(0);

                    break;
                }
            case "SniperBoi":
                {
                    GiveWeapon(1);
                    break;
                }
            case "MachinegunBoi":
                {
                    GiveWeapon(2);
                    break;
                }
            case "ShotgunBoi":
                {
                    GiveWeapon(3);
                    break;
                }
        }
    }
    private void GiveWeapon(int ChosenWeapon)
    {
        Weapons[ChosenWeapon].transform.SetParent(PlaceForGun);
        Weapons[ChosenWeapon].transform.position = PlaceForGun.position;
        switch (ChosenWeapon)
        {
            case 0:
                {
                    Weapons[ChosenWeapon].GetComponent<Pistol>().PlayerCam = Player.transform.GetChild(0).GetComponent<Camera>();
                    break;
                }
            case 1:
                {
                    Weapons[ChosenWeapon].GetComponent<SniperRifle>().PlayerCam = Player.transform.GetChild(0).GetComponent<Camera>();
                    break;
                }
            case 2:
                {
                    Weapons[ChosenWeapon].GetComponent<MachineGun>().PlayerCam = Player.transform.GetChild(0).GetComponent<Camera>();
                    break;
                }
            case 3:
                {
                    Weapons[ChosenWeapon].GetComponent<Shotgun>().PlayerCam = Player.transform.GetChild(0).GetComponent<Camera>();
                    var rotationVector = Weapons[ChosenWeapon].transform.rotation.eulerAngles;
                    rotationVector.y = 90;
                    Weapons[ChosenWeapon].transform.rotation = Quaternion.Euler(rotationVector);
                    break;
                }
        }
        //gameObject.GetComponent<PlayerMovement>().speed = 1f;
    }
}
