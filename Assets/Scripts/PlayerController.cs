using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Rough player controller including necessary values and a state machine to manager player actions.

    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping,
        Attacking,
        Blocking,
        Throwing,
        SpecialMove,
        Damaged,
        Stunned,
        Win,
        Lose
    }

    /*
    Player variables listed below manage the following:
    Whether this character is controlled by player 1 or 2 (used to determine which input to recognize),
    Current state of the player character,
    Player character's initial health points,
    The player character's speed value (probably uniform across all characters)
    */

    public int playerID;
    public Vector2 startPosition;

    public PlayerState currentState;

    public float healthPoints = 100f;
    public float playerSpeed = 5f;

    private CharacterController controller;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private SpriteRenderer playerSprite;
    private Vector2 movementInput = Vector2.zero;

    private bool isAttacking = false;
    private bool attackTriggered = false;

    private bool combatActionActive = false; //bool to indicate if the player is current involved in a combat action

    private bool isBlocking = false;
    private bool blockTriggered = false;

    private bool isThrowing = false;
    private bool throwTriggered = false;

    public Sprite attackSprite;
    public Sprite idleSprite;
    public Sprite throwSprite;
    public Sprite blockSprite;

    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        if (playerID == 2)
        {
            playerSprite.flipX = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!combatActionActive)
            movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking && !attackTriggered && !combatActionActive)
            attackTriggered = true;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed && !isBlocking && !blockTriggered && !combatActionActive)
            blockTriggered = true;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed && !isThrowing && !throwTriggered && !combatActionActive)
            throwTriggered = true;
    }

    private void Update()
    {
        UpdateState(movementInput);
        PerformStateActions();
    }

    /*
    This function manages how the player character switches states based on player input.
    */
    private void UpdateState(Vector2 moveInput /*float verticalInput, bool attackInput, bool blockInput, bool throwInput, bool specialMoveInput*/)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                if (moveInput.x != 0)
                {
                    currentState = PlayerState.Walking;
                }
                else if (attackTriggered && !isAttacking)
                {
                    currentState = PlayerState.Attacking;
                    StartCoroutine(EndAttack());
                }
                
                else if (blockTriggered && !isBlocking)
                {
                    currentState = PlayerState.Blocking;
                    StartCoroutine(EndBlock());
                }
                else if (throwTriggered && !isThrowing)
                {
                    currentState = PlayerState.Throwing;
                    StartCoroutine(EndThrow());
                }
                /*
                else if (specialMoveInput)
                {
                    currentState = PlayerState.SpecialMove;
                }*/
                break;

            case PlayerState.Walking:
                if (movementInput.x == 0)
                {
                    currentState = PlayerState.Idle;
                }
                break;

            case PlayerState.Attacking:
                break;
        }
    }

    private void PerformStateActions()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                Debug.Log("Player is idle.");
                playerSprite.sprite = idleSprite;
                break;
            case PlayerState.Walking:
                Vector3 currentPos = gameObject.transform.position;
                Vector3 move = new Vector3(movementInput.x, 0, 0);
                Debug.Log(movementInput);
                //controller.Move(move * Time.deltaTime * playerSpeed);
                gameObject.transform.position = (currentPos + (move * Time.deltaTime * playerSpeed));
                Debug.Log("Player is walking.");
                break;
            case PlayerState.Attacking:
                Debug.Log("Player is attacking.");
                break;
            case PlayerState.Blocking:
                Debug.Log("Player is blocking.");
                break;
            case PlayerState.Throwing:
                Debug.Log("Player is throwing.");
                break;
            case PlayerState.SpecialMove:
                Debug.Log("Player is using a special move.");
                break;
            case PlayerState.Damaged:
                Debug.Log("Player is damaged.");
                break;
            case PlayerState.Lose:
                Debug.Log("Player lost.");
                break;
            case PlayerState.Win:
                Debug.Log("Player won.");
                break;
        }

    }

    private IEnumerator EndAttack()
    {
        combatActionActive = true;
        isAttacking = true;
        playerSprite.sprite = attackSprite;
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Attack has ended.");

        isAttacking = false;
        attackTriggered = false;
        combatActionActive = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator EndBlock()
    {
        combatActionActive = true;
        isBlocking = true;
        playerSprite.sprite = blockSprite;
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Block has ended.");

        isBlocking = false;
        blockTriggered = false;
        combatActionActive = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator EndThrow()
    {
        combatActionActive = true;
        isThrowing = true;
        playerSprite.sprite = throwSprite;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Throw has ended.");

        isThrowing = false;
        throwTriggered = false;
        combatActionActive = false;
        currentState = PlayerState.Idle;
    }

    public void TakeDamage(float damageAmount)
    {
        healthPoints -= damageAmount;

        if (healthPoints <= 0)
            currentState = PlayerState.Lose;
    }


}
