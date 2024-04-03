using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI announcerTextLine1;
    public TextMeshProUGUI announcerTextLine2;
    public TextMeshProUGUI battleTimer;

    public Slider[] healthBars;

    public GameObject[] winTrackerGrids;
    public GameObject winTracker;

    public static BattleUI instance;
    public static BattleUI GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void AddWin(int player)
    {
        GameObject go = Instantiate(winTracker, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winTrackerGrids[player].transform);
    }
}
