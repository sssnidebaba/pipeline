using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutputArea: MonoBehaviour
{
    /// <summary>
    /// 初始要求颜色序列
    /// </summary>
    public List<Color> aimBallCols;
    public Sprite cirRight, cirFalse, cirOri;//输入正确，错误贴图
    public Transform aimBall;//球（目标）

    /// <summary>
    /// 当前显示输出要求的球序列
    /// </summary>
    private List<Transform> aimBalls = new List<Transform>();

    /// <summary>
    /// 当前输出范围内的球序列
    /// </summary>
    [SerializeField]
    private List<Transform> inBalls=new List<Transform>();

    /// <summary>
    /// 当前范围内的球和球序列的哈希映射
    /// </summary>
    private Hashtable cirtoPoint=new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        InitBuildAimballs();
    }
    #region 初始化生成目标球序列
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

    #region 范围内球序列更新


    //球进入时更新
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

    //球离开时更新
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
