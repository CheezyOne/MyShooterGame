using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private float gravity = -12f;
    private float speed = 4f;
    private float standartVelocityY = -2f;
    private float jumpHeight = 3f;
    private Vector3 scaleToSmall =new Vector3(0.6f, 0.6f, 0.6f);
    private Vector3 scaleToNormal = new Vector3(1f, 1f, 1f);
    private Vector3 velocity;


    private bool decreaseSpeedCrouch = true;
    private bool increaseSpeedRun = false;
    public bool isGrounded=true;

    void Update()
    {
        
        if (GetComponent<OutOfBounds>().WentOutOfBounds)
        {
            return;
        }
        if(controller.isGrounded )
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false; 
        }
        if(isGrounded && velocity.y <0)
        {
            velocity.y = standartVelocityY;
        }

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        
        Vector3 Movement = transform.right * Horizontal + transform.forward * Vertical;

        controller.Move(Movement * speed*Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(increaseSpeedRun)
            {
                speed += 3f;
                increaseSpeedRun = false;
            }
        }
        else
        {
            if(!increaseSpeedRun)
            {
                increaseSpeedRun = true;
                speed = 3f;
            }
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (decreaseSpeedCrouch)
            {
                decreaseSpeedCrouch = false;
                speed--;
            }
            transform.localScale = scaleToSmall;
        }
        else
        {
            if (!decreaseSpeedCrouch)
            {
                decreaseSpeedCrouch = true;
                speed++;
            }
            transform.localScale = scaleToNormal;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight*-2f*gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
