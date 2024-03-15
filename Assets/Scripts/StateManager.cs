using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public int health = 100;

    public float horizontal;
    public float vertical;
    public bool attackAction;
    public bool blockAction;
    public bool throwAction;

    public bool canAttack;
    public bool gettingHit;
    public bool currentlyAttacking;

    public bool dontMove;
    public bool onGround; //possibly unnecessary if jumping is not included
    public bool lookRight;

    public Slider healthSlider;
    SpriteRenderer sRenderer;

    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;

    private void Start()
    {
        handleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        sRenderer.flipX = lookRight;
    }
}
