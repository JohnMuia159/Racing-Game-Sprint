using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    PlayerMovement m_player;
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallRunTime, maxWallSpeed;
    bool isWallRight, isWallLeft, isWallRunning;


    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        WallRunInput();

    }

    public void WallRunInput()
    {
        //Start Wallrun
        if (Input.GetKey(KeyCode.D) && isWallRight)
            StartWallRun();
        if (Input.GetKey(KeyCode.A) && isWallLeft)
            StartWallRun();

    }
    public void StartWallRun()
    {
        m_player.gravity = 0;
        isWallRunning = true;
        if (m_player.velocity.magnitude <= maxWallSpeed)
        {
            m_player.controller.Move(Vector3.forward * wallRunForce * Time.deltaTime);
            if (isWallRight)
                m_player.controller.Move(Vector3.right * wallRunForce / 5 * Time.deltaTime);
            else
                m_player.controller.Move(Vector3.left * wallRunForce / 5 * Time.deltaTime);
        }

    }

    public void StopWallRun()
    {
        m_player.gravity = -9.8f;
        isWallRunning = false;
    }
    public void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, Vector3.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, Vector3.left, 1f, whatIsWall);
    }

}
