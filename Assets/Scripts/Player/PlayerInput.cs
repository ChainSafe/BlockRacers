using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player input manager
/// </summary>
public class PlayerInput : MonoBehaviour
{
    #region Fields
    
    // Player Input
    private PlayerInputActions playerInput;
    // Player controller
    private PlayerController playerController;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes our input actions and listens, needed for the new input system
    /// </summary>
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

    /// <summary>
    /// Steering input
    /// </summary>
    /// <param name="context">context</param>
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        var currentMovementInput = context.ReadValue<Vector2>();
        playerController.HorizontalInput = currentMovementInput.x;
    }
    
    /// <summary>
    /// Acceleration input
    /// </summary>
    /// <param name="context">context</param>
    private void OnAccelerateInput(InputAction.CallbackContext context)
    {
        playerController.VerticalInput = context.ReadValue<float>();
    }
    
    /// <summary>
    /// Brake input
    /// </summary>
    /// <param name="context">context</param>
    private void OnBrakeInput(InputAction.CallbackContext context)
    {
        playerController.IsBraking = Convert.ToBoolean(context.ReadValue<float>());
        playerController.VerticalInput = context.ReadValue<float>() * -1;
    }

    /// <summary>
    /// Drift input
    /// </summary>
    /// <param name="context">context</param>
    private void OnDriftInput(InputAction.CallbackContext context)
    {
        playerController.IsDrifting = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    /// <summary>
    /// Nos input
    /// </summary>
    /// <param name="context">context</param>
    private void OnNosInput(InputAction.CallbackContext context)
    {
        PlayerController.nosActive = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    /// <summary>
    /// Reset input
    /// </summary>
    /// <param name="context">context</param>
    private void OnResetInput(InputAction.CallbackContext context)
    {
        playerController.resetActive = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    /// <summary>
    /// Used for player movement, call this to enable input detection
    /// </summary>
    private void OnEnable()
    {
        playerInput.Enable();
    }
    
    /// <summary>
    /// Used for player movement, call this to disable input detection
    /// </summary>
    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    #endregion
}
