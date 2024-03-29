using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int numberOfUsers;
    public List<PlayerBase> players = new List<PlayerBase>();

    public List<CharacterBase> characterList = new List<CharacterBase>();

    public CharacterBase returnCharacterWithID(string id)
    {
        CharacterBase returnValue = null;

        for (int i= 0; i < characterList.Count; i++)
        {
            if (string.Equals(characterList[i].charID,id))
            {
                returnValue = characterList[i];
            }
        }

        return returnValue;
    }

    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase returnValue = null;

        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].playerStates == states)
            {
                returnValue = players[i];
                break;
            }
        }

        return returnValue;
    }

    // CharacterManager class is a singleton, can be accessed anywhere by calling GetInstance function below
    public static CharacterManager instance;
    public static CharacterManager GetInstance()
    {
        return instance;
    }

    // Set up instance in Awake, also set instance to carry over between scenes
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

[System.Serializable]
public class PlayerBase
{
    public string playerID;
    public string inputID;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    public enum PlayerType
    {
        user,
        ai,
        simulation
    }
}

[System.Serializable]
public class CharacterBase
{
    public string charID;
    public GameObject prefab;
}