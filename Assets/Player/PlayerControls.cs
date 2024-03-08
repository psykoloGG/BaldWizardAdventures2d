using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR;

/**
 * PlayerControls
 *
 * Class responsible for all player controls:
 * - Movement
 * - Hand rotation and shooting
 */
public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer PlayerSpriteRenderer;
    
    [SerializeField]
    private SpriteRenderer HandSpriteRenderer;
    
    // Movement controls
    [SerializeField]
    private Rigidbody2D PlayerRigidbody;
    private PlayerInputActions PlayerInput;

    private InputAction MoveAction;
    private InputAction FireAction;

    // Movement parameters
    [SerializeField]
    private float MoveSpeed = 1.0f;
    
    private Vector2 MoveDirection; // Determined by PlayerInput
    [SerializeField]
    private bool VerticalMovementAllowed = false; // Used to disable vertical movement (e.g. on ladder = true)

    //Hand rotation mechanics
    [SerializeField]
    private GameObject HandRotator;
    
    [SerializeField]
    private Camera MainCamera;
    private Vector3 MousePosition;
    
    private void OnEnable()
    {
        MoveAction.Enable();
        FireAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
        FireAction.Disable();
    }

    private void Awake()
    {
        PlayerInput = new PlayerInputActions();
        MoveAction = PlayerInput.Player.Move;
        FireAction = PlayerInput.Player.Fire;
        FireAction.performed += Ctx => Fire();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateHandRotation();
    }

    private void UpdateMovement()
    {
        MoveDirection = MoveAction.ReadValue<Vector2>();
        PlayerRigidbody.velocity = new Vector2(MoveDirection.x * MoveSpeed, MoveDirection.y * MoveSpeed);
        
        // Adjust sprite to face direction player is moving
        if (MoveDirection.x > 0)
        {
            PlayerSpriteRenderer.flipX = false;
        }
        else if (MoveDirection.x < 0)
        {
            PlayerSpriteRenderer.flipX = true;
        }
    }

    private void UpdateHandRotation()
    {
        MousePosition = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 LookDirection = MousePosition - HandRotator.transform.position;
        float Angle = Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;
        HandRotator.transform.rotation = Quaternion.Euler(0, 0, Angle);
        
        if (Angle > 90 || Angle < -90)
        {
            HandSpriteRenderer.flipY = true;
        }
        else
        {
            HandSpriteRenderer.flipY = false;
        }
    }
    private void Fire()
    {
        Debug.Log("fired");
    }

}