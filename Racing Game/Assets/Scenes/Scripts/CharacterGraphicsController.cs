using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGraphicsController : MonoBehaviour
{
    private SkinnedMeshRenderer m_skinnedMesh;
    private Animator m_animator;
    private PlayerMovement m_player;

    private int isGrounded;
    private int isWalking;
    private int isRunning;


    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerMovement>();
        m_skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        m_animator = GetComponentInChildren<Animator>();

        isGrounded = Animator.StringToHash("isGrounded");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool(isGrounded, m_player.IsGrounded());
        m_animator.SetBool(isWalking, m_player.IsWalking());
        m_animator.SetBool(isRunning, m_player.IsRunning());

    }
}
