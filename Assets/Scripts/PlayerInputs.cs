using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private InputSettings inputs;
    private bool jump = false;
    private Vector2 moveDirection;
    private PlayerController controller;
    private bool jumpButtonHeld = false;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        inputs = new InputSettings();
        inputs.Enable();
        inputs.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Jump()
    {
        jump = true;
    }


    private void FixedUpdate()
    {
        controller.Move(inputs.Player.Move.ReadValue<Vector2>(), jump);
        jump = false;
        bool jumpButton = inputs.Player.Jump.ReadValue<float>() != 0;
        if (jumpButtonHeld && !jumpButton)
            controller.JumpButtonReleased();
        jumpButtonHeld = jumpButton;
    }
}
