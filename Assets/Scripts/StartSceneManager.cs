using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public bool loadingLevel;

    public void TriggerLevelLoad()
    {
        if (!loadingLevel)
        {
            Debug.Log("Loading level...");
            loadingLevel = true;
            StartCoroutine("LoadLevel");
        }
    }

    public void SetCharacterCount()
    {
        CharacterManager.GetInstance().numberOfUsers = 2;
        CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlayerType.user;
    }

    IEnumerator LoadLevel()
    {
        SetCharacterCount();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Select", LoadSceneMode.Single);

    }
}
