using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutputArea: MonoBehaviour
{
    /// <summary>
    /// ��ʼҪ����ɫ����
    /// </summary>
    public List<Color> aimBallCols;
    public Sprite cirRight, cirFalse, cirOri;//������ȷ��������ͼ
    public Transform aimBall;//��Ŀ�꣩

    /// <summary>
    /// ��ǰ��ʾ���Ҫ���������
    /// </summary>
    private List<Transform> aimBalls = new List<Transform>();

    /// <summary>
    /// ��ǰ�����Χ�ڵ�������
    /// </summary>
    [SerializeField]
    private List<Transform> inBalls=new List<Transform>();

    /// <summary>
    /// ��ǰ��Χ�ڵ���������еĹ�ϣӳ��
    /// </summary>
    private Hashtable cirtoPoint=new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        InitBuildAimballs();
    }
    #region ��ʼ������Ŀ��������
    private void InitBuildAimballs()
    {
        for (int i = 0; i < aimBallCols.Count; i++)
        {
            var ls = transform.TransformPoint(new Vector2(4, 3.8f) - i * new Vector2(1.2f, 0));
            aimBalls.Add(Instantiate(aimBall, ls, Quaternion.identity, transform));
            aimBalls[i].GetComponent<SpriteRenderer>().color = aimBallCols[i];
        }
    }
    #endregion 

    #region ��Χ�������и���


    //�����ʱ����
    private void OnTriggerEnter(Collider collision)
    {
        cirEnter(collision);
    }
    private void cirEnter(Collider collision)
    {
        if (collision.tag == "Ball")
        {
            if (inBalls.Count == 0)
            {
                var tmp = Instantiate(aimBall, transform.TransformPoint(new Vector2(4, 6f)), Quaternion.identity, transform);
                tmp.GetComponent<SpriteRenderer>().color = collision.GetComponent<SpriteRenderer>().color;
                inBalls.Add(tmp);
                cirtoPoint.Add(collision, tmp);
            }
            else
            {
                var tmp = Instantiate(aimBall, transform.TransformPoint(new Vector2(4, 6f) - inBalls.Count * new Vector2(1.2f, 0)), Quaternion.identity, transform);
                tmp.GetComponent<SpriteRenderer>().color = collision.GetComponent<SpriteRenderer>().color;
                inBalls.Add(tmp);
                cirtoPoint.Add(collision, tmp);
            }
        }
    }

    //���뿪ʱ����
    private void OnTriggerExit(Collider collision)
    {
        cirExit(collision);
    }
    private void cirExit(Collider collision)
    {
        if (collision.tag == "Ball")
        {
            var tmp = (Transform)cirtoPoint[collision];
            for (int i = inBalls.FindIndex(a => a == tmp) + 1; i < inBalls.Count; i++)
            {
                inBalls[i].position = transform.TransformPoint(new Vector2(4, 6f) - (i - 1) * new Vector2(1.2f, 0));
            }
            inBalls.Remove(tmp);
            Destroy(tmp.gameObject);
            cirtoPoint.Remove(collision);
        }
    }
    #endregion
}
