using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController characterController;
    Vector3 velocity;

    [Header("Movement")]
    public float moveSpeed;


    [Header("Jumping")]
    public float gravity;
    public float jumpHeight;
    public float groundDistance;
    public LayerMask groundMask;
    public Transform groundChecker;
    public bool isGrounded;


    [Header("Leaning")]
    public Transform leaner;
    public bool isLeaning;
    public float leaningSmooth;
    float zRotation;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        Gravity();
        Jump();
        Leaning();
    }

    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    private void Leaning()
    {
        if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            bool isLeaningLeft = Input.GetKey(KeyCode.Q);
            zRotation = Mathf.Lerp(zRotation, isLeaningLeft ? 20f : -20f, leaningSmooth * Time.deltaTime);
            isLeaning = true;
        }
        if(Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            isLeaning = false;
        }
        if (!isLeaning)
        {
            zRotation = Mathf.Lerp(zRotation, 0f, leaningSmooth * Time.deltaTime);
        }
        leaner.localRotation = Quaternion.Euler(0, 0, zRotation);
    }

}
