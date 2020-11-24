using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    CharacterController col;
    PlayerMovement m_player;
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallRunTime, maxWallSpeed, maxWallRunTimer;
    public bool isWallRight, isWallLeft, isWallRunning, wallJumped;
    public int wallJumpCounter;

    // Start is called before the first frame update
    void Start()
    {

        col = GetComponent<CharacterController>();
        m_player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_player.isGhosted)
            whatIsWall = LayerMask.GetMask("Wall", "Objects");
        else if (m_player.isGhosted)
            whatIsWall = LayerMask.GetMask("Wall");

        CheckForWall();
        WallRunInput();
        WallJumps();

        maxWallRunTimer -= Time.deltaTime;

        if (isWallRunning)
            if (isWallLeft && Input.GetKeyDown(KeyCode.Space) && !wallJumped)
            {
                m_player.velocity.y = Mathf.Sqrt(m_player.jumpHeight * -2 * -9.8f);
                wallJumpCounter++;
                wallJumped = true;
            }
            else if (isWallRight && Input.GetKeyDown(KeyCode.Space) && !wallJumped)
            {
                m_player.velocity.y = Mathf.Sqrt(m_player.jumpHeight * -2 * -9.8f);
                wallJumpCounter++;
                wallJumped = true;
            }


        if (m_player.IsGrounded() && !isWallRunning)
            maxWallRunTimer = maxWallRunTime;

    }


    public void WallRunInput()
    {
        //Start Wallrun
        if (isWallRight && !m_player.IsGrounded())
            StartWallRun();
        if (isWallLeft && !m_player.IsGrounded())
            StartWallRun();

    }
    public void StartWallRun()
    {

        isWallRunning = true;
       // m_player.velocity.y += -m_player.gravity * -1.5f / (float)wallJumpCounter * Time.deltaTime;
        if (m_player.velocity.magnitude <= maxWallSpeed)
        {
            m_player.controller.Move(transform.forward * wallRunForce / 2 * Time.deltaTime);
            if (isWallRight)
                m_player.controller.Move(transform.right * wallRunForce / 7 * Time.deltaTime);
            else
                m_player.controller.Move(-transform.right * wallRunForce / 7 * Time.deltaTime);
        }

    }

    public void StopWallRun()
    {
        isWallRunning = false;
        WallRunInput();
        wallJumped = false;
    }
    public void CheckForWall()
    {
        isWallRight = Physics.Raycast(new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z), transform.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z), -transform.right, 1f, whatIsWall);
        Debug.DrawRay(new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z), transform.right, Color.red);
        Debug.DrawRay(new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z), -transform.right, Color.red);
  

        //leave wall run
        if (!isWallLeft && !isWallRight || m_player.IsGrounded())
            StopWallRun();

    }

    public void WallJumps()
    {
        if (wallJumped)
        {
            WallRunInput();
        }
    }

}
