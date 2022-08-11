using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BallControl : MonoBehaviour
{
    /// <summary>
    /// �����ʼ������
    /// </summary>
    public InitData initdata = new InitData();
    private void Start()
    {
        PlayButton.startPlay.TurnOn += StartWork;
        PlayButton.startPlay.TurnOff += EndWork;

        //��ȡ��ʼλ�úͽǶ�
        initdata.initRo = transform.rotation;
        initdata.initPos = transform.position;
    }

    #region ����������
    private void StartWork(object sender, EventArgs e)
    {
        initdata.initRo = transform.rotation;
        initdata.initPos = transform.position;
        transform.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<SphereCollider>().isTrigger = false;
    }
    private void EndWork(object sender, EventArgs e)
    {
        var rig = transform.GetComponent<Rigidbody>();
        rig.velocity = new Vector2(0, 0);
        rig.angularVelocity = new Vector3(0,0,0);
        rig.useGravity = false;

        transform.position = initdata.initPos;
        GetComponent<SphereCollider>().isTrigger = true;
    }
    #endregion
}