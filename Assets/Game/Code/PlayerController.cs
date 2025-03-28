using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    public Rigidbody rb;
    public Animator animator;
    public CharacterStats characterStats;
    
    [Foldout("Movement Module", foldEverything = true, styled = true, readOnly = true)]
    public float inputMagnitude;
    public bool isControlsActive = true;

    private Vector3 _moveVector;

    private void FixedUpdate()
    {
        MoveCharacter();        
    }

    private void MoveCharacter()
    {
        if(!isControlsActive)
        {
            inputMagnitude = 0;
            animator.SetBool("isWalking", false);
            animator.SetFloat("movementSpeed", 0);
            return;
        }
        
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight   = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y   = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 inputDirection = cameraForward * Input.GetAxis("Vertical") + cameraRight   * Input.GetAxis("Horizontal");

        if (inputDirection.magnitude > 1f)
        {
            inputDirection.Normalize();
        }

        _moveVector = inputDirection * characterStats.moveSpeed.Value * Time.deltaTime;

        if (inputDirection.magnitude > 0)
        {
            inputMagnitude = inputDirection.magnitude;

            Vector3 movementDirection = Vector3.RotateTowards(rb.transform.forward, _moveVector, characterStats.rotateSpeed.Value * Time.deltaTime, 0.0f);
            rb.transform.rotation = Quaternion.LookRotation(movementDirection);

            animator.SetBool("isWalking", true);
            animator.SetFloat("movementSpeed", inputMagnitude);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("movementSpeed", 0);
        }
        
        rb.MovePosition(rb.position + _moveVector);
    }
}