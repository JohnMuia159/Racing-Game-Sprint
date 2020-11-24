using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGraphicsController : MonoBehaviour
{
    private SkinnedMeshRenderer m_skinnedMesh;
    private Animator m_animator;
    private PlayerMovement m_player;
    private WallRun wallRun;

    private int isGrounded;
    private int isWalking;
    private int isRunning;
    private int isWallRunning;
    private int isWallRightProperty;
    private int isWallLeftProperty;


    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerMovement>();
        m_skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        m_animator = GetComponentInChildren<Animator>();

        isGrounded = Animator.StringToHash("isGrounded");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        isWallRunning = Animator.StringToHash("isWallRunning");
        isWallRightProperty = Animator.StringToHash("isWallRight");
        isWallLeftProperty = Animator.StringToHash("isWallLeft");

    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool(isWallRunning, m_player.IsWallRunning());
        m_animator.SetBool(isWallLeftProperty, m_player.IsWallLeft());
        m_animator.SetBool(isWallRightProperty, m_player.IsWallRight());


        m_animator.SetBool(isGrounded, m_player.IsGrounded());
        m_animator.SetBool(isWalking, m_player.IsWalking());
        m_animator.SetBool(isRunning, m_player.IsRunning());


    }
}
