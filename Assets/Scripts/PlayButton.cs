using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    public static PlayButton startPlay;
    public event EventHandler TurnOn,TurnOff;//����/���� �¼�
    public Sprite sprOn, sprOff;//��ťͼ��
    private bool on;//�Ƿ���
 
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
    //����¼�
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
    //�����¼�
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
    //�����¼�


}
