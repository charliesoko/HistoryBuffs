using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text AnnouncerTextLine1;
    public Text AnnouncerTextLine2;
    public Text BattleTimer;

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
