using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovement : MonoBehaviour
{
    Rigidbody2D rb;
    StateManager states;

    public float acceleration;
    public float airAcceleration;
    public float maxSpeed;
    public float jumpSpeed;
    public float jumpDuration;
    float actualSpeed;
    bool justJumped;
    bool canVariableJump;
    float jumpTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<StateManager>();

        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        
    }
}
