using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    public int numberOfPlayers;
    public List<PlayerInterfaces> playerInterfaces = new List<PlayerInterfaces>();
    public SelectIconInfo[] iconPrefabs;
    public int maxX;
    public int maxY;
    SelectIconInfo[,] charGrid;

    public GameObject iconCanvas;

    bool loadingLevel;
    public bool bothPlayersSelected;

    CharacterManager charManager;

    #region Singleton
    public static SelectSceneManager instance;
    public static SelectSceneManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        charManager = CharacterManager.GetInstance();
        numberOfPlayers = charManager.numberOfUsers;

        charGrid = new SelectIconInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        iconPrefabs = iconCanvas.GetComponentsInChildren<SelectIconInfo>();

        for (int i = 0; i < iconPrefabs.Length; i++)
        {
            iconPrefabs[i].xPos += x;
            iconPrefabs[i].yPos += y;

            charGrid[x, y] = iconPrefabs[i];

            if (x < maxX - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    private void Update()
    {
        if (!loadingLevel)
        {
            for (int i=0; i < playerInterfaces.Count; i++)
            {
                if (i < numberOfPlayers)
                {
                    if (!charManager.players[i].hasCharacter)
                    {
                        playerInterfaces[i].playerBase = charManager.players[i];

                        HandleSelectorPosition(playerInterfaces[i]);
                        HandleSelectInput(playerInterfaces[i], charManager.players[i].inputID);
                        HandleCharacterPreview(playerInterfaces[i]);
                    }
                }
                else
                {
                    charManager.players[i].hasCharacter = true;
                }
            }
        }

        if(bothPlayersSelected)
        {
            Debug.Log("Loading level...");
            StartCoroutine("LoadLevel");
            loadingLevel = true;
        }
        else
        {
            if (charManager.players[0].hasCharacter && charManager.players[1].hasCharacter)
            {
                bothPlayersSelected = true;
            }
        }
    }

    IEnumerator LoadLevel()
    {
        for (int i = 0; i < charManager.players.Count; i++)
        {
            if (charManager.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                if(charManager.players[i].playerPrefab == null)
                {
                    int randomVal = Random.Range(0, iconPrefabs.Length);

                    charManager.players[i].playerPrefab = charManager.returnCharacterWithID(iconPrefabs[randomVal].characterID).prefab;

                    Debug.Log(iconPrefabs[randomVal].characterID);
                }
            }    
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
    }

    void HandleSelectorPosition(PlayerInterfaces p1)
    {
        p1.selector.SetActive(true);

        p1.activeIcon = charGrid[p1.activeX, p1.activeY];

        Vector2 selectorPosition = p1.activeIcon.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(iconCanvas.transform.localPosition.x, iconCanvas.transform.localPosition.y);

        p1.selector.transform.localPosition = selectorPosition;
    }

    void HandleCharacterPreview(PlayerInterfaces p1)
    {
        if (p1.previewIcon != p1.activeIcon)
        {
            if (p1.createdChar != null)
            {
                Destroy(p1.createdChar);
            }
        }

        if (p1.createdChar == null)
        {
            GameObject go = Instantiate(CharacterManager.GetInstance().returnCharacterWithID(p1.activeIcon.characterID).prefab, p1.charPreviewPos.position, Quaternion.identity) as GameObject;

            p1.createdChar = go;
        }

        p1.previewIcon = p1.activeIcon;

        /*
        if (!string.Equals(p1.playerBase.playerID, charManager.players[0].playerID))
        {
            p1.createdChar.GetComponent<PlayerController>().lookRight = true;
        }
        */
    }

    void HandleSelectInput(PlayerInterfaces p1, string playerID)
    {
        #region Grid Navigation

        float vertical = Input.GetAxis("Vertical" + playerID);

        if (vertical !=0)
        {
            if (!p1.hitInputOnce)
            {
                if (vertical > 0)
                {
                    p1.activeY = (p1.activeY > 0) ? p1.activeY - 1 : maxY - 1;
                }
                else
                {
                    p1.activeY = (p1.activeY < maxY - 1) ? p1.activeY + 1 : 0;
                }

                p1.hitInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerID);

        if (horizontal != 0)
        {
            if (!p1.hitInputOnce)
            {
                if (horizontal > 0)
                {
                    p1.activeX = (p1.activeX > 0) ? p1.activeX - 1 : maxX - 1;
                }
                else
                {
                    p1.activeX = (p1.activeX < maxX - 1) ? p1.activeX + 1 : 0;
                }

                p1.timerToReset = 0;
                p1.hitInputOnce = true;
            }
        }

        if (vertical == 0 && horizontal == 0)
        {
            p1.hitInputOnce = false;
        }

        if (p1.hitInputOnce)
        {
            p1.timerToReset += Time.deltaTime;

            if (p1.timerToReset > 0.8f)
            {
                p1.hitInputOnce = false;
                p1.timerToReset = 0;
            }
        }

        #endregion

        if (Input.GetButtonUp("Select" + playerID))
        {
            p1.playerBase.playerPrefab = charManager.returnCharacterWithID(p1.activeIcon.characterID).prefab;

            p1.playerBase.hasCharacter = true;
        }
    }

    [System.Serializable]
    public class PlayerInterfaces
    {
        public SelectIconInfo activeIcon;
        public SelectIconInfo previewIcon;
        public GameObject selector;
        public Transform charPreviewPos;
        public GameObject createdChar;

        public int activeX;
        public int activeY;

        public bool hitInputOnce;
        public float timerToReset;

        public PlayerBase playerBase;
    }
}
