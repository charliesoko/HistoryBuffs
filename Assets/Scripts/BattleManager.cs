using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    WaitForSeconds oneSec; //To avoid creating a new WaitForSeconds every time it's needed
    public Transform[] spawnPositions; //Array holding the spawn position of each player

    CharacterManager charManager;
    BattleUI battleUI;

    public int maxRounds; //Number of rounds needed to win
    int currentRound = 1; //Current round we are in

    //Round countdown variables
    public bool countdown;
    public int maxRoundTimer;
    int currentTimer;
    float internalTimer;

    private void Start()
    {
        //Get singleton references
        charManager = CharacterManager.GetInstance();
        battleUI = BattleUI.GetInstance();

        //Initialize WaitForSeconds to be used later on
        oneSec = new WaitForSeconds(1);

        //Start with announcer text disabled
        battleUI.announcerTextLine1.gameObject.SetActive(false);
        battleUI.announcerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");
    }

    private void Update()
    {
        //Check if the round countdown has been triggered and start method to handle
        if (countdown)
        {
            HandleRoundTimer();
        }
    }

    IEnumerator StartGame()
    {
        //Create the player characters
        //yield return CreatePlayers();

        //Start the round
        yield return InitializeRound();
    }

    IEnumerator CreatePlayers()
    {
        //Pull players from list in CharacterManager
        for (int i = 0; i < charManager.players.Count; i++)
        {
            //Instantiate player prefabs
            GameObject go = Instantiate(charManager.players[i].playerPrefab, spawnPositions[i].position, Quaternion.identity) as GameObject;

            //Get references to player StateManagers and health sliders in scene
            charManager.players[i].playerStates = go.GetComponent<StateManager>();
            charManager.players[i].playerStates.healthSlider = battleUI.healthBars[i];
        }

        yield return null;
    }

    IEnumerator InitializeRound()
    {
        //Ensure announcer text is disabled at the start of the round
        battleUI.announcerTextLine1.gameObject.SetActive(false);
        battleUI.announcerTextLine2.gameObject.SetActive(false);

        //Reset the round timer
        currentTimer = maxRoundTimer;
        countdown = false;

        //Initialize players each round
        //yield return InitializePlayers();

        //Enable player controls
        yield return EnableControls();
    }

    IEnumerator InitializePlayers()
    {
        for (int i = 0; i < charManager.players.Count; i++)
        {
            //Initialize player health and spawn location each round; adjust later on to use health value that can be adjusted in inspector rather than hard-coded
            charManager.players[i].playerStates.health = 100;
            //initialize animation
            charManager.players[i].playerStates.transform.position = spawnPositions[i].position;
        }

        yield return null;
    }

    IEnumerator EnableControls()
    {
        TextMeshProUGUI text1 = battleUI.announcerTextLine1;
        
        //Start round with announcer text, beginning with the round number and then counting down to the round start
        text1.gameObject.SetActive(true);
        text1.fontSize = 60;
        text1.text = ("Round " + currentRound);
        text1.color = Color.white;
        yield return oneSec;
        yield return oneSec;

        //Text color and size increases as it counts down
        text1.color = Color.green;
        //text1.fontSize = 36;
        text1.text = ("3");
        yield return oneSec;

        text1.color = Color.yellow;
        //text1.fontSize = text1.fontSize + 6;
        text1.text = ("2");
        yield return oneSec;

        text1.color = Color.red;
        //text1.fontSize = text1.fontSize + 8;
        text1.text = ("1");
        yield return oneSec;

        text1.color = Color.red;
        //text1.fontSize = text1.fontSize + 10;
        text1.text = ("FIGHT!");
        yield return oneSec;

        /*Insert code here to enable player controls - 
        find a link between how the new scripts handle character controls
        and how the Input System is used in the original script*/

        //Disable announcer text after countdown
        yield return oneSec;
        text1.gameObject.SetActive(false);
        countdown = true;
    }

    void DisableControls()
    {
        //Insert code to disable controls
    }

    void HandleRoundTimer()
    {
        battleUI.battleTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime;

        if (internalTimer > 1)
        {
            currentTimer--;
            internalTimer = 0;
        }

        if (currentTimer <= 10)
        {
            battleUI.battleTimer.color = Color.red;
        }

        if (currentTimer <= 0)
        {
            EndRoundCheck(true);
            countdown = false;
        }
    }

    public void EndRoundCheck(bool timeOut = false)
    {
        //Function is used to end the round and determine if it was due to the time running out
        countdown = false;

        battleUI.battleTimer.text = maxRoundTimer.ToString();
        battleUI.battleTimer.color = Color.white;

        //If the round ends on a time out, display the appropriate message
        if (timeOut)
        {
            battleUI.announcerTextLine1.gameObject.SetActive(true);
            battleUI.announcerTextLine1.color = Color.red;
            battleUI.announcerTextLine1.fontSize = 90;
            battleUI.announcerTextLine1.text = ("Time!");
        }
        else
        {
            battleUI.announcerTextLine1.gameObject.SetActive(true);
            battleUI.announcerTextLine1.color = Color.red;
            battleUI.announcerTextLine1.fontSize = 90;
            battleUI.announcerTextLine1.text = ("K.O.");
        }

        //Disable player controls
        DisableControls();

        //Begin the end of the round
        StartCoroutine("EndRound");
    }

    IEnumerator EndRound()
    {
        //Delay to allow round end text to display
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
    }
}
