using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    public static PlayButton startPlay;
    public event EventHandler TurnOn,TurnOff;//启动/重置 事件
    public Sprite sprOn, sprOff;//按钮图标
    private bool on;//是否按下
 
    private void Awake()
    {
        startPlay = this;    
    }
    public void click()
    {
        if (!on)
        {
            StartWork();
        }
        else
        {
            Endwork();
        }
    }
    //点击事件
    private void StartWork()
    {
        if (TurnOn != null)
        {
            transform.GetComponent<Image>().sprite = sprOff;
            TurnOn(this,EventArgs.Empty);
            on = true;
            GameMode.gameMode = GameMode.gamemode.onPlay;
        }
    }
    //启动事件
    private void Endwork()
    {
        if (TurnOff != null)
        {
            transform.GetComponent<Image>().sprite = sprOn;
            TurnOff(this, EventArgs.Empty);
            on = false;
            GameMode.gameMode = GameMode.gamemode.onBoard;
        }
      
    }
    //重置事件


}
