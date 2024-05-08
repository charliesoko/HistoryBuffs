using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
        DamagedB,
        DamagedC,
        DamagedD,
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
    public string characterName;

    public PlayerState currentState;

    public float healthPoints = 10000f;
    public UnityEngine.UI.Slider healthSlider;
    public bool lookRight = true;

    public float playerSpeed = 5f;
    public float damageAmount = 20f;

    private CharacterController controller;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private BoxCollider2D HitboxD;
    private BoxCollider2D HurtboxD;
    private SpriteRenderer playerSprite;
    private Vector2 movementInput = Vector2.zero;

    public PlayerInput playerInput;
    public bool controlsEnabled;

    public GameObject opponentPlayer;

    public GameObject HitA;
    public GameObject HurtA;

    public GameObject HitB;
    public GameObject HurtB;

    public GameObject HitC;
    public GameObject HurtC;

    public GameObject HitD;
    public GameObject HurtD;

    public Vector2 pushBack;
    public Vector2 opponentPushBack;

    private bool isAttacking = false;
    private bool isAttackingB = false;
    private bool isAttackingC = false;
    private bool attackTriggered = false;
    private bool attackTriggeredB = false;
    private bool attackTriggeredC = false;

    private bool combatActionActive = false; //bool to indicate if the player is current involved in a combat action
    private bool combatActionActiveB = false;
    private bool combatActionActiveC = false;

    public float delayAHitBActivate;
    public float delayAHurtBActivate;
    public float delayAHitBDeactivate;
    public float delayAHurtBDeactivate;

    public float delayBHitBActivate;
    public float delayBHurtBActivate;
    public float delayBHitBDeactivate;
    public float delayBHurtBDeactivate;

    public float delayCHitBActivate;
    public float delayCHurtBActivate;
    public float delayCHitBDeactivate;
    public float delayCHurtBDeactivate;

    public float delayDHitBActivate;
    public float delayDHurtBActivate;
    public float delayDHitBDeactivate;
    public float delayDHurtBDeactivate;

    public float hitStunA;
    public float hitStunB;
    public float hitStunC;
    public float hitStunD;

    private bool isBlocking = false;
    private bool blockTriggered = false;

    private bool isThrowing = false;
    private bool throwTriggered = false;

    public Sprite attackSprite;
    public Sprite attackBSprite;
    public Sprite attackCSprite;
    public Sprite idleSprite;
    public Sprite throwSprite;
    public Sprite blockSprite;
    public Sprite hitRecieved;
    public Sprite lossSprite;
    public Sprite winSprite;

    public AudioClip[] hitSFX;
    public AudioSource sfxSource;

    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        HitboxD = HitD.GetComponent<BoxCollider2D>();
        HurtboxD = HurtD.GetComponent<BoxCollider2D>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        sfxSource = gameObject.GetComponent<AudioSource>();


        /*if (playerID == 2)
        {
            playerSprite.flipX = true;
        }*/

        HurtA.SetActive(false);
        HitA.SetActive(false);
        HurtB.SetActive(false);
        HitB.SetActive(false);
        HurtC.SetActive(false);
        HitC.SetActive(false);
        HurtD.SetActive(false);
        HitD.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (!combatActionActive)
            movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (currentState != PlayerState.Damaged && currentState != PlayerState.DamagedB && currentState != PlayerState.DamagedC && currentState != PlayerState.DamagedD)
        {
            if (context.performed && !isAttacking && !attackTriggered && !combatActionActive)
                attackTriggered = true;
        }
    }
    public void OnAttackB(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (currentState != PlayerState.Damaged && currentState != PlayerState.DamagedB && currentState != PlayerState.DamagedC && currentState != PlayerState.DamagedD)
        {
            if (context.performed && !isAttackingB && !attackTriggeredB && !combatActionActiveB)
                attackTriggeredB = true;
        }
    }
    public void OnAttackC(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (currentState != PlayerState.Damaged && currentState != PlayerState.DamagedB && currentState != PlayerState.DamagedC && currentState != PlayerState.DamagedD)
        {
            if (context.performed && !isAttackingC && !attackTriggeredC && !combatActionActiveC)
                attackTriggeredC = true;
        }
    }
    public void OnBlock(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (currentState != PlayerState.Damaged && currentState != PlayerState.DamagedB && currentState != PlayerState.DamagedC && currentState != PlayerState.DamagedD)
        {
            if (context.performed && !isBlocking && !blockTriggered && !combatActionActive)
                blockTriggered = true;


            if (context.canceled)
            {
                Debug.Log("Block has ended.");

                isBlocking = false;
                blockTriggered = false;
                combatActionActive = false;
                currentState = PlayerState.Idle;
            }
        }

        HurtA.SetActive(false);
        HitA.SetActive(false);
        HurtB.SetActive(false);
        HitB.SetActive(false);
        HurtC.SetActive(false);
        HitC.SetActive(false);
        HurtD.SetActive(false);
        HitD.SetActive(false);
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!controlsEnabled)
            return;

        if (currentState != PlayerState.Damaged && currentState != PlayerState.DamagedB && currentState != PlayerState.DamagedC && currentState != PlayerState.DamagedD)
        {
            if (context.performed && !isThrowing && !throwTriggered && !combatActionActive)
                throwTriggered = true;
        }
    }

    private void Update()
    {
        UpdateState(movementInput);
        PerformStateActions();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SceneManager.LoadScene("SampleScene");
        }

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("CharacterMenu");
        }*/

        if (healthPoints < 1)
        {
            currentState = PlayerState.Lose;
        }
    }

    private void FixedUpdate()
    {
        if (playerID != 1)
            playerSprite.flipX = true;

        if (healthSlider != null)
        {
            healthSlider.value = healthPoints; //* 0.01f
        }

        if (healthPoints < 1)
        {
            if (BattleManager.GetInstance().countdown)
            {
                BattleManager.GetInstance().EndRoundCheck();
            }
        }
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

                    HurtA.SetActive(false);
                    HitA.SetActive(false);
                    HurtB.SetActive(false);
                    HitB.SetActive(false);
                    HurtC.SetActive(false);
                    HitC.SetActive(false);
                    HurtD.SetActive(false);
                    HitD.SetActive(false);

                }
                else if (attackTriggered && !isAttacking)
                {
                    currentState = PlayerState.Attacking;
                    StartCoroutine(AttackA());
                }
                else if (attackTriggeredB && !isAttackingB)
                {
                    currentState = PlayerState.Attacking;
                    StartCoroutine(AttackB());
                }
                else if (attackTriggeredC && !isAttackingC)
                {
                    currentState = PlayerState.Attacking;
                    StartCoroutine(AttackC());
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

                    HurtA.SetActive(false);
                    HitA.SetActive(false);
                    HurtB.SetActive(false);
                    HitB.SetActive(false);
                    HurtC.SetActive(false);
                    HitC.SetActive(false);
                    HurtD.SetActive(false);
                    HitD.SetActive(false);

                }
                break;

            case PlayerState.Damaged:
                StartCoroutine(HitStunA());
                break;

            case PlayerState.DamagedB:
                StartCoroutine(HitStunB());
                break;

            case PlayerState.DamagedC:
                StartCoroutine(HitStunC());
                break;

            case PlayerState.DamagedD:
                StartCoroutine(HitStunD());
                break;

            case PlayerState.Attacking:
                break;

            case PlayerState.Lose:
                playerSprite.sprite = lossSprite;

                /*if(healthPoints >= 1)
                {
                    currentState = PlayerState.Idle;
                }*/

                break;

            case PlayerState.Win:
                playerSprite.sprite = winSprite;
                break;
        }
    }

    private void PerformStateActions()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                ////Debug.Log("Player is idle.");
                playerSprite.sprite = idleSprite;
                break;
            case PlayerState.Walking:
                Vector3 currentPos = gameObject.transform.position;
                Vector3 move = new Vector3(movementInput.x, 0, 0);
                ////Debug.Log(movementInput);
                //controller.Move(move * Time.deltaTime * playerSpeed);
                if (controlsEnabled)
                {
                    gameObject.transform.position = (currentPos + (move * Time.deltaTime * playerSpeed));
                    playerSprite.sprite = idleSprite;
                }
                else
                    gameObject.transform.position = currentPos;
                ////Debug.Log("Player is walking.");
                break;
            case PlayerState.Attacking:
                ////Debug.Log("Player is attacking.");
                break;
            case PlayerState.Blocking:
                ////Debug.Log("Player is blocking.");
                break;
            case PlayerState.Throwing:
                ////Debug.Log("Player is throwing.");
                break;
            case PlayerState.SpecialMove:
                Debug.Log("Player is using a special move.");
                break;
            case PlayerState.Damaged:
                playerSprite.sprite = hitRecieved;
                ////Debug.Log("Player is damaged.");
                break;
            case PlayerState.DamagedB:
                playerSprite.sprite = hitRecieved;
                ////Debug.Log("Player is damaged.");
                break;
            case PlayerState.DamagedC:
                playerSprite.sprite = hitRecieved;
                ////Debug.Log("Player is damaged.");
                break;
            case PlayerState.DamagedD:
                playerSprite.sprite = lossSprite;
                ////Debug.Log("Player is damaged.");
                break;
            case PlayerState.Lose:
                Debug.Log("Player lost.");
                break;
            case PlayerState.Win:
                Debug.Log("Player won.");
                break;
        }

    }

    private IEnumerator AttackA()
    {
        combatActionActive = true;
        isAttacking = true;


        yield return new WaitForSeconds(delayAHurtBActivate);
        HurtA.SetActive(true);


        yield return new WaitForSeconds(delayAHitBActivate);
        HitA.SetActive(true);
        playerSprite.sprite = attackSprite;


        yield return new WaitForSeconds(delayAHitBDeactivate);
        HitB.SetActive(false);
        HitA.SetActive(false);
        HitC.SetActive(false);


        yield return new WaitForSeconds(delayAHurtBDeactivate);
        HurtB.SetActive(false);
        HurtC.SetActive(false);
        HurtA.SetActive(false);

        ////Debug.Log("Attack has ended.");

        isAttacking = false;
        attackTriggered = false;
        combatActionActive = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator AttackB()
    {
        combatActionActiveB = true;
        isAttackingB = true;

        yield return new WaitForSeconds(delayBHurtBActivate);
        HurtB.SetActive(true);

        yield return new WaitForSeconds(delayBHitBActivate);
        playerSprite.sprite = attackBSprite;
        HitB.SetActive(true);

        yield return new WaitForSeconds(delayBHitBDeactivate);
        HitB.SetActive(false);
        HitA.SetActive(false);
        HitC.SetActive(false);

        yield return new WaitForSeconds(delayBHurtBDeactivate);
        HurtB.SetActive(false);
        HurtC.SetActive(false);
        HurtA.SetActive(false);

        ////Debug.Log("Attack has ended.");

        isAttackingB = false;
        attackTriggeredB = false;
        combatActionActiveB = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator AttackC()
    {
        combatActionActiveC = true;
        isAttackingC = true;

        yield return new WaitForSeconds(delayCHurtBActivate);
        HurtC.SetActive(true);

        yield return new WaitForSeconds(delayCHitBActivate);
        playerSprite.sprite = attackCSprite;
        HitC.SetActive(true);

        yield return new WaitForSeconds(delayCHitBDeactivate);
        HitB.SetActive(false);
        HitA.SetActive(false);
        HitC.SetActive(false);

        yield return new WaitForSeconds(delayCHurtBDeactivate);
        HurtB.SetActive(false);
        HurtC.SetActive(false);
        HurtA.SetActive(false);

        ////Debug.Log("Attack has ended.");

        isAttackingC = false;
        attackTriggeredC = false;
        combatActionActiveC = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator EndBlock()
    {
        combatActionActive = true;
        isBlocking = true;
        playerSprite.sprite = blockSprite;
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator EndThrow()
    {
        combatActionActive = true;
        isThrowing = true;

        yield return new WaitForSeconds(delayDHurtBActivate);
        HurtD.SetActive(true);

        yield return new WaitForSeconds(delayDHitBActivate);
        HitD.SetActive(true);
        playerSprite.sprite = throwSprite;

        yield return new WaitForSeconds(delayDHitBDeactivate);
        HitD.SetActive(false);

        yield return new WaitForSeconds(delayDHurtBDeactivate);
        HurtD.SetActive(false);

        Debug.Log("Throw has ended.");

        isThrowing = false;
        throwTriggered = false;
        combatActionActive = false;
        currentState = PlayerState.Idle;
    }

    private IEnumerator HitStunA()
    {

        playerSprite.sprite = hitRecieved;

        yield return new WaitForSeconds(hitStunA);

        if (currentState == PlayerState.Damaged)
        {
            currentState = PlayerState.Idle;
        }
    }

    private IEnumerator HitStunB()
    {

        playerSprite.sprite = hitRecieved;

        yield return new WaitForSeconds(hitStunB);

        if (currentState == PlayerState.DamagedB)
        {
            currentState = PlayerState.Idle;
        }
    }

    private IEnumerator HitStunC()
    {
        playerSprite.sprite = hitRecieved;

        yield return new WaitForSeconds(hitStunC);

        if (currentState == PlayerState.DamagedC)
        {
            currentState = PlayerState.Idle;
        }
    }

    private IEnumerator HitStunD()
    {
        playerSprite.sprite = lossSprite;

        yield return new WaitForSeconds(hitStunD);

        if (currentState == PlayerState.DamagedD)
        {
            currentState = PlayerState.Idle;
        }
    }

    //public void TakeDamage(float damageAmount)
    //{
    //    healthPoints -= damageAmount;

    //    if (healthPoints <= 0)
    //        currentState = PlayerState.Lose;
    //}

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("HurtboxP2") && collider.gameObject.transform.parent.CompareTag("Player2"))
        {

            if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState == PlayerState.Blocking)
            {
                if (currentState == PlayerState.Throwing)
                {
                    rigidbody.AddForce(pushBack * 0f);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack + new Vector2(opponentPushBack.x * 4, opponentPushBack.y + 200));
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedD;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                }
                else
                {
                    rigidbody.AddForce(pushBack);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack);
                }
            }
            else if (currentState == PlayerState.Throwing)
            {
                if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState != PlayerState.DamagedD)
                {
                    rigidbody.AddForce(pushBack * 0f);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack + new Vector2(opponentPushBack.x * 4, opponentPushBack.y + 200));
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedD;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                }
            }
            else
            {
                rigidbody.AddForce(pushBack * 0.2f);
                collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack * 4);

                if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState != PlayerState.DamagedD)
                {
                    if (HitA.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.Damaged;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 400;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);

                    }
                    else if (HitB.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedB;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);

                    }
                    else if (HitC.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedC;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1800;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                    }
                }
            }

            Debug.Log("The move has Hit!");
        }


        else if (collider.gameObject.CompareTag("HurtboxP1") && collider.gameObject.transform.parent.CompareTag("Player1"))
        {

            if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState == PlayerState.Blocking)
            {
                if (currentState == PlayerState.Throwing)
                {
                    rigidbody.AddForce(pushBack * 0f);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack + new Vector2(opponentPushBack.x * 4, opponentPushBack.y + 200));
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedD;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                }
                else
                {
                    rigidbody.AddForce(pushBack);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack);
                }
            }
            else if (currentState == PlayerState.Throwing)
            {
                if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState != PlayerState.DamagedD)
                {
                    rigidbody.AddForce(pushBack * 0f);
                    collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack + new Vector2(opponentPushBack.x * 4, opponentPushBack.y + 200));
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedD;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                    collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                }
            }
            else
            {
                rigidbody.AddForce(pushBack * 0.2f);
                collider.gameObject.transform.parent.GetComponent<Rigidbody2D>().AddForce(opponentPushBack * 4);


                if (collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState != PlayerState.DamagedD)
                {
                    if (HitA.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.Damaged;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 400;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);

                    }
                    else if (HitB.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedB;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1000;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);

                    }
                    else if (HitC.activeSelf)
                    {
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().currentState = PlayerState.DamagedC;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().healthPoints -= 1800;
                        collider.gameObject.transform.parent.GetComponent<PlayerController>().sfxSource.PlayOneShot(hitSFX[Random.Range(0, 5)]);
                    }
                }
            }

            Debug.Log("The move has Hit!");
        }
    }
}
