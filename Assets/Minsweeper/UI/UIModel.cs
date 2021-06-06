using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModel
{
    public int BombCount;
    public int BombsFound;
    //public int SecondsPassed;
    float Timer;

    public UIModel()
    {
        Timer = 0;
    }

    public string flagcount(int value)
    {
        BombsFound += value;
        return string.Format("Flag: {0}/{1}", BombsFound, BombCount);
    }

    public string UpdateSecondpassed()
    {
        Timer += Time.deltaTime;
        return "Timer : " + Mathf.FloorToInt(Timer).ToString();
    }
}
