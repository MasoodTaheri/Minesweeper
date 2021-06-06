using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresenter
{
    UIModel UIModel = new UIModel();
    private IuiView uiview;

    public UIPresenter(IuiView view, int bombcount)
    {
        uiview = view;
        UIModel.BombCount = bombcount;
    }

    //public void ConnectBetweenModelAndView()
    //{
    //    UIModel.BombCount=
    //}
    public void UpdateTimer()
    {
        uiview.Timer = UIModel.UpdateSecondpassed();
    }

    public void UpdateFlagCount(int value)
    {
        uiview.flag = UIModel.flagcount(value);
    }

}
