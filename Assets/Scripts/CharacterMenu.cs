using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{

    private string characterName;

    private bool p1Turn = true;

    private bool p2Turn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void P1Select()
    {
        characterName = gameObject.name;
        p1Turn = false;
        p2Turn = true;
    }
}
