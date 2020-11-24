using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public WallRun wallrun;

    public bool isWalkingDisabled;
    public GameObject[] objects;

    //Speed Powerup
    public bool speedUpPower;
    public bool pressedPowerUp;
    public float powerUpTime = 4f;
    public float powerUpTimer = 0f;

    //Ghost Powerup
    public bool ghostPowerUp;
    public bool isGhosted;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public Vector3 velocity;

    private CapsuleCollider col;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private LayerMask ghostMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public void Start()
    {
        wallrun = GetComponent<WallRun>();
        col = GetComponent<CapsuleCollider>();
        groundMask = LayerMask.GetMask("Ground", "Terrain");
        ghostMask = LayerMask.GetMask("Objects");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsGrounded()
    {
        if( Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            return true;
        }
        return false;
    }

    public bool IsWalking()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 && !IsRunning() || Input.GetAxisRaw("Horizontal") == -1 && !IsRunning() || Input.GetAxisRaw("Vertical") == 1 && !IsRunning() || Input.GetAxisRaw("Vertical") == -1 && !IsRunning())
        {
            if (!speedUpPower)
                speed = 6;
            else if (speedUpPower && pressedPowerUp)
            {
                isWalkingDisabled = true;
            }
            return true;
        }
        return false;
    }

    public bool IsRunning()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 && Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("Horizontal") == -1 && Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("Vertical") == 1 && Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("Vertical") == -1 && Input.GetKey(KeyCode.LeftShift))
        {
            if (!speedUpPower)
                speed = 8;
            else if (speedUpPower && pressedPowerUp)
                speed = 14;
            return true;
        }
        else if (isWalkingDisabled)
        {
            speed = 14;
            return true;
        }     
        return false;
    }

    public bool IsWallRunning()
    {
        if (wallrun.isWallRunning)
            return true;
        else
            return false;
    }

    public bool IsWallLeft()
    {
        if (wallrun.isWallLeft && !wallrun.isWallRight)
            return true;
        else
            return false;
    }
    public bool IsWallRight()
    {
        if (wallrun.isWallRight && !wallrun.isWallLeft)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        //jump
        IsGrounded();
        IsWalking();
        IsRunning();
        Jump();

        objects = GameObject.FindGameObjectsWithTag("Objects");

        powerUpTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && speedUpPower)
            pressedPowerUp = true;
        else if (Input.GetKeyDown(KeyCode.Z) && ghostPowerUp)
        {
            pressedPowerUp = true;
            isGhosted = true;
        }


        if (ghostPowerUp && pressedPowerUp)
        {
            if (Physics.CheckSphere(transform.position, 15, ghostMask))
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].GetComponent<BoxCollider>().isTrigger = true;
                }
            }
        } else if (!ghostPowerUp)
        {
            if (Physics.CheckSphere(transform.position, 15, ghostMask))
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }

        if (!speedUpPower && !pressedPowerUp || speedUpPower && !pressedPowerUp || !ghostPowerUp && !pressedPowerUp || ghostPowerUp && !pressedPowerUp)
            powerUpTimer = powerUpTime;

        if (powerUpTimer < 0)
        {
            speedUpPower = false;
            isWalkingDisabled = false;
            pressedPowerUp = false;
            ghostPowerUp = false;
            isGhosted = false;
        }

        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
            wallrun.wallJumped = false;
        }

        if (wallrun.wallJumped)
        {
            velocity.y += gravity * 05f * Time.deltaTime;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(groundCheck.position, groundDistance);
    //    Gizmos.DrawSphere(transform.position, 15);
    //    Gizmos.DrawLine(transform.position, Vector3.right);
    //    Gizmos.DrawLine(transform.position, Vector3.left);
    //}
}