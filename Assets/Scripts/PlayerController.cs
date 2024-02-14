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
    public int playerNumber;
    public Vector2 startPosition;

    public PlayerState currentState;

    public float healthPoints = 100f;
    public float playerSpeed = 5f;

    private CharacterController controller;
    private SpriteRenderer renderer;
    private Vector2 movementInput = Vector2.zero;
    private bool attacked;

    public Sprite attackSprite;
    public Sprite idleSprite;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && currentState != PlayerState.Attacking)
            attacked = true;
    }

    private void Update()
    {
        UpdateState(movementInput);
        PerformStateActions();
    }

    /*
    This function manages how the player character switches states based on player input.
    */
    private void UpdateState(Vector2 movementInput /*float verticalInput, bool attackInput, bool blockInput, bool throwInput, bool specialMoveInput*/)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                if (movementInput.x != 0)
                {
                    currentState = PlayerState.Walking;
                }
                else if (attacked)
                {
                    currentState = PlayerState.Attacking;
                }
                /*
                else if (blockInput)
                {
                    currentState = PlayerState.Blocking;
                }
                else if (throwInput)
                {
                    currentState = PlayerState.Throwing;
                }
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
                if (!attacked)
                {
                    currentState = PlayerState.Idle;
                }
                break;
        }
    }

    private void PerformStateActions()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                Debug.Log("Player is idle.");
                renderer.sprite = idleSprite;
                break;
            case PlayerState.Walking:
                Vector3 move = new Vector3(movementInput.x, 0, 0);
                Debug.Log(movementInput);
                controller.Move(move * Time.deltaTime * playerSpeed);
                Debug.Log("Player is walking.");
                break;
            case PlayerState.Attacking:
                renderer.sprite = attackSprite;     
                StartCoroutine(EndAttack());
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
        yield return new WaitForSeconds(1.0f);
        attacked = false;

        Debug.Log("Attack has ended.");
    }

    public void TakeDamage(float damageAmount)
    {
        healthPoints -= damageAmount;

        if (healthPoints <= 0)
            currentState = PlayerState.Lose;
    }


}
