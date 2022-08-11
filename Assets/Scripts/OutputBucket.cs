using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OutputBucket : MonoBehaviour
{
    /// <summary>
    /// ���Ҫ��������
    /// </summary>
    private List<Transform> aimBalls = new List<Transform>();
    /// <summary>
    /// Ҫ�������ɫ����
    /// </summary>
    public List<Color> aimBallCols;
    /// <summary>
    /// ����������
    /// </summary>
    private List<Transform> curBalls = new List<Transform>();

    public Transform aimBall;//prefab 
    public Sprite cirright, cirfalse,cirOri;//������ȷ��������ͼ,��ʼ��ͼ

    private int cirnum;//��Χ��������
    private Vector2 curBallpos=new Vector2(0,-5f);//���������г�ʼ����λ��
    
    void Start()
    {
        InitBuildAimballs();
        PlayButton.startPlay.TurnOff += EndWork;
    }
    private void OnDestroy()
    {
        PlayButton.startPlay.TurnOff -= EndWork;
    }
    #region ��ʼ������Ŀ��������
    /// <summary>
    /// ��ʼ������Ҫ���������
    /// </summary>
    private void InitBuildAimballs()
    {
        for (int i = 0; i < aimBallCols.Count; i++)
        {
            var ls = transform.TransformPoint(new Vector2(0, -1.5f) + i * new Vector2(0, 1.5f));
            aimBalls.Add(Instantiate(aimBall, ls, Quaternion.identity, transform));
            aimBalls[i].GetComponent<SpriteRenderer>().color = aimBallCols[i];
        }
    }
    #endregion

    #region Ŀ�������и��� X ���������и���
    private void OnTriggerEnter(Collider collision)
    {
        if (GameMode.gameMode == GameMode.gamemode.onPlay)
        {
            if (collision.tag == "Ball")
            { 
                cirEnter(collision);
                CurBallsUpdata(collision);
            }
        }
    }
    /// <summary>
    /// Ŀ�������и���
    /// </summary>
    private void cirEnter(Collider collision)
    {
        cirnum++;
        if (cirnum <= aimBallCols.Count)
        {
            if (collision.GetComponent<SpriteRenderer>().color == aimBallCols[cirnum - 1])
            {
                aimBalls[cirnum - 1].GetComponent<SpriteRenderer>().sprite = cirright;
            }
            else
            {
                aimBalls[cirnum - 1].GetComponent<SpriteRenderer>().sprite = cirfalse;
                aimBalls[cirnum - 1].rotation = Quaternion.Euler(0, 0, 45);
            }
        }
    }
    /// <summary>
    /// ���������и���
    /// </summary>
    private void CurBallsUpdata(Collider collision)
    {
        var tmppos = (Vector2)transform.TransformPoint(curBallpos);
        var tmpBall = Instantiate(aimBall, tmppos + (cirnum - 1) * new Vector2(1, 0), Quaternion.identity, transform);
        tmpBall.GetComponent<SpriteRenderer>().color = collision.GetComponent<SpriteRenderer>().color;
        curBalls.Add(tmpBall);
    }
    #endregion
    #region �ָ���ʼ
    /// <summary>
    /// �ָ���ʼ
    /// </summary>
    private void EndWork(object sender, EventArgs e)
    {
        cirnum = 0;
        foreach (var ls in aimBalls)
        {
            ls.GetComponent<SpriteRenderer>().sprite = cirOri;
        }
        foreach (var ls in curBalls)
        {
            Destroy(ls.gameObject);
        }
        curBalls.Clear();
    }
    #endregion
}
