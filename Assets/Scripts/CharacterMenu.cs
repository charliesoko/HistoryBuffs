using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{

    private string P1characterName = "Picking...";

    private string P2characterName = "Picking...";

    private bool p1Turn = true;

    private bool p2Turn = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Player 1 is: {P1characterName}");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (p1Turn == true && p2Turn == false)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (p1Turn == false && p2Turn == true)
            {
                p1Turn = true;
                p2Turn = false;
                P1characterName = "Picking...";
                Debug.Log($"Player 1 is: {P1characterName}");
            }
        }

    }

    public void PlayerSelect()
    {
        if (p2Turn == true)
        {
            P2characterName = EventSystem.current.currentSelectedGameObject.name;
            p2Turn = false;
            Debug.Log($"Player 2 is: {P2characterName}");
            SceneManager.LoadScene("SampleScene");
        }

        if (p1Turn == true)
        {
            P1characterName = EventSystem.current.currentSelectedGameObject.name;
            p1Turn = false;
            p2Turn = true;

            Debug.Log($"Player 1 is: {P1characterName}");
        }

    }
}
