using UnityEngine;

public class HeadShaking : MonoBehaviour
{
    [SerializeField] bool IsEnabled = true;

    [SerializeField, Range(0, 0.1f)] float Amplitude = 0.015f;
    [SerializeField, Range(0, 30)] float Frequency = 10f;

    [SerializeField] Transform Camera = null;

    private float ToggleSpeed = 0.4f;
    private Vector3 StartPos;
    private CharacterController Controller;

    void Awake()
    {
        StartPos = Camera.localPosition;
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller == null)
            return;
        if (!IsEnabled)
            return;
        CheckMotion();
        ResetPosition();
        Camera.LookAt(FocusTarget());
    }
    Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * Frequency) * Amplitude ;
        pos.x += Mathf.Cos(Time.time * Frequency / 2) * Amplitude / 3f;
        return pos;
    }
    void CheckMotion()
    {
        float speed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if (speed < ToggleSpeed)
            return;
        if (!Controller.isGrounded)
            return;
        PlayMotion(FootStepMotion());
    }
    void ResetPosition()
    {
        if (Camera.localPosition == StartPos) return;
        Camera.localPosition = Vector3.Lerp(Camera.localPosition, StartPos, 1 * Time.deltaTime);
    }
    Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + Camera.localPosition.y, transform.position.z);
        pos += Camera.forward * 15f;
        return pos;
    }
    void PlayMotion(Vector3 motion)
    {
        Camera.localPosition += motion;
    }
}