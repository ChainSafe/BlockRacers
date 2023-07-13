using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // Player Input
    private PlayerInputActions playerInput;
    
    // Player controller
    private PlayerController playerController;

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        // Initialize player input actions
        playerInput = new PlayerInputActions();
        playerInput.Game.Move.started += OnMovementInput;
        playerInput.Game.Move.canceled += OnMovementInput;
        playerInput.Game.Move.performed += OnMovementInput;
        playerInput.Game.Nos.started += OnNosInput;
        playerInput.Game.Nos.canceled += OnNosInput;
        playerInput.Game.Nos.performed += OnNosInput;
        playerInput.Game.Accelerate.started += OnAccelerateInput;
        playerInput.Game.Accelerate.canceled += OnAccelerateInput;
        playerInput.Game.Accelerate.performed += OnAccelerateInput;
        playerInput.Game.Brake.started += OnBrakeInput;
        playerInput.Game.Brake.canceled += OnBrakeInput;
        playerInput.Game.Brake.performed += OnBrakeInput;
        playerInput.Game.Drift.started += OnDriftInput;
        playerInput.Game.Drift.canceled += OnDriftInput;
        playerInput.Game.Drift.performed += OnDriftInput;
        playerInput.Game.Reset.performed += OnResetInput;
    }
    
    // used for player movement, call this to enable or disable player input detection
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    // Steering input
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        var currentMovementInput = context.ReadValue<Vector2>();
        playerController.horizontalInput = currentMovementInput.x;
        //verticalInput = currentMovementInput.y;
    }
    
    // Accelerate input
    private void OnAccelerateInput(InputAction.CallbackContext context)
    {
        playerController.verticalInput = context.ReadValue<float>();
    }
    
    // Brake input
    private void OnBrakeInput(InputAction.CallbackContext context)
    {
        playerController.isBraking = Convert.ToBoolean(context.ReadValue<float>());
        playerController.verticalInput = context.ReadValue<float>() * -1;
    }

    // Drift input
    private void OnDriftInput(InputAction.CallbackContext context)
    {
        playerController.isDrifting = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    // Nos input
    private void OnNosInput(InputAction.CallbackContext context)
    {
        PlayerController.nosActive = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    // Reset input
    private void OnResetInput(InputAction.CallbackContext context)
    {
        playerController.resetActive = Convert.ToBoolean(context.ReadValue<float>());
    }
}
