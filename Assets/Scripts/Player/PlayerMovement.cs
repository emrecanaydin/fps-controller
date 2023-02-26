using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    CharacterController characterController;
    Vector3 velocity;

    [Header("Movement")]
    float defaultMoveSpeed;
    public float moveSpeed;
    public bool isRunning;
    public List<AudioClip> walkingSounds = new List<AudioClip>();
    public List<AudioClip> runningSounds = new List<AudioClip>();
    public List<AudioClip> jumpingSounds = new List<AudioClip>();
    float movementSoundsTimer;
    AudioSource audioSource;


    [Header("Jumping")]
    public float gravity;
    public float jumpForce;
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
        defaultMoveSpeed = moveSpeed;
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Movement();
        Run();
        Gravity();
        Jump();
        Leaning();
    }

    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (isLeaning)
        {
            return;
        }
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        if(move != Vector3.zero && isGrounded)
        {
            MovementSounds();
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isRunning)
        {
            isRunning = true;
            DOTween.To(() => moveSpeed, x => moveSpeed = x, moveSpeed * 1.5f, 2);
            //moveSpeed = moveSpeed * 1.50f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            DOTween.To(() => moveSpeed, x => moveSpeed = x, defaultMoveSpeed, 2);
            //moveSpeed = defaultMoveSpeed;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            velocity.y = jumpForce;
            audioSource.PlayOneShot(jumpingSounds[Random.Range(0, jumpingSounds.Count)]);
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

    private void MovementSounds()
    {
        movementSoundsTimer -= Time.deltaTime;
        if(movementSoundsTimer <= 0)
        {
            if (isRunning)
            {
                audioSource.PlayOneShot(runningSounds[Random.Range(0, runningSounds.Count)]);
                movementSoundsTimer = .5f;
            } else
            {
                audioSource.PlayOneShot(walkingSounds[Random.Range(0, walkingSounds.Count)]);
                movementSoundsTimer = .6f;
            }
        }
    }

}
