using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer Line;
    [SerializeField] Transform StartPosition;
    [SerializeField] Transform EndPosition;
    private bool ShouldFire = true;
    void Start()
    {
        Line.SetPosition(0, StartPosition.position);
        Line.SetPosition(1, EndPosition.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if( other.TryGetComponent<PlayerHP>(out PlayerHP PlayerHealth))
        {
            PlayerHealth.takeDamage(25);
            //Play Sound
        }
    }
    void Update()
    {
        if(!ShouldFire)
        {
            Line.SetPosition(0, Vector3.zero);
            Line.SetPosition(1, Vector3.zero);
        }
    }
}
