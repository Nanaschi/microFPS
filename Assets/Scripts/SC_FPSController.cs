using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    [SerializeField] private Camera playerCamera;

    public Camera PlayerCamera => playerCamera;

    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;


    [SerializeField] private Image _aimImage;

    public event Action<GameObject> OnCursorDragged;


    [SerializeField] private ParticleSystem _hitParticleSystem;
    
    private void OnEnable()
    {
        OnCursorDragged += CursorColorChange;
        EnemyBehaviour.OnPlayerHit += PlayParticleSystem;
    }

    private void PlayParticleSystem(EnemyBehaviour enemyBehaviour)
    {
        _hitParticleSystem.Play();
    }

    private void CursorColorChange(GameObject objectUnderCursor)
    {
        if (objectUnderCursor.GetComponent<EnemyBehaviour>())
        {
            _aimImage.color = Color.yellow;
        }
        else
        {
            _aimImage.color = Color.red;
        }
    }

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        CursorDetection();

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void CursorDetection()
    {
        RaycastHit raycastHit;

        if (Physics.Raycast
                (playerCamera.transform.position, playerCamera.transform.forward, out raycastHit))
        {
            OnCursorDragged?.Invoke(raycastHit.transform.gameObject);
        }
        else
        {
            _aimImage.color = Color.red;
        }
    }
    
    
    private void OnDisable()
    {
        OnCursorDragged -= CursorColorChange;
        EnemyBehaviour.OnPlayerHit -= PlayParticleSystem;
    }
    
}