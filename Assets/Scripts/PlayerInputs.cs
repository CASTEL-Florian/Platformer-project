using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private InputSettings inputs;

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Jump()
    {
        
    }
}
