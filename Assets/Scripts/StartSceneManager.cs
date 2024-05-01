using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public bool loadingLevel;
    CharacterManager charManager;
    public Image fadeBackground;
    public bool doFade;

    private void Start()
    {
        charManager = CharacterManager.GetInstance();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (doFade)
            StartCoroutine(FadeIn(fadeBackground));
        else
            fadeBackground.enabled = false;

        if (fadeBackground.color.a == 0)
        {
            fadeBackground.enabled = false;
        }
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    public float fadeTime;
    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = c;
        }
    }

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
        SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);

    }
}
