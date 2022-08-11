using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LineControl : MonoBehaviour
{
    public Transform start,end,col,cirpre;//prefab
    public LineRenderer body;//线的身体
    public List<Transform> cirs;//线上的锚点列表
    public bool isdrawing;//是否正在画线
    public bool oncollision;//画线过程中是否与与其他物体重叠
    public bool onRay;//鼠标是否在线上
    public bool pointOn;//是否连接锚点
    public InitData initdata = new InitData();//初始数据
    public int colnum = 0;//重叠物体数量

    [SerializeField]
    private int cirnum=5;//锚点数量


    bool isStart = false;
    private void Start()
    {
        Debug.Log(Time.frameCount);
        int index = 1;
        //初始化锚点数量
        for (int i = cirs.Count; i < 5; i++)
        {
            var tempCir = Instantiate(cirpre, (start.position + end.position) * 0.5f, Quaternion.identity, transform);
            cirs.Add(tempCir);
            tempCir.name = index+"";
            index++;
        }
        //设置图层关系
        start.GetComponent<SpriteRenderer>().sortingOrder = 1;
        end.GetComponent<SpriteRenderer>().sortingOrder = 1;
        body.sortingOrder = 1;

        PlayButton.startPlay.TurnOn += StartWork;
        PlayButton.startPlay.TurnOff += EndWork;

        initdata.initRo = transform.rotation;
        initdata.initPos = transform.position;

        isStart = true;
    }
    void Update()
    {
        BodyUpdata();
        Ondrawupdataline();
        Onrayupdataline();
    }

    #region 启动和重置
    private void StartWork(object sender, EventArgs e)
    {
        //设置重力和碰撞器状态
        var rig = transform.GetComponent<Rigidbody>();
        if (pointOn)
        {
            rig.isKinematic = false;
            rig.useGravity = true;
        }
        else
        {
            rig.isKinematic = true;
            rig.useGravity = false;
        }
        
        transform.Find("start").GetComponent<SphereCollider>().isTrigger = false;
        transform.Find("end").GetComponent<SphereCollider>().isTrigger = false;
        transform.Find("body").GetComponent<BoxCollider>().isTrigger = false;
    }
    private void EndWork(object sender, EventArgs e)
    {
        //设置重力和碰撞器状态
        var rig = transform.GetComponent<Rigidbody>();
        if (rig.useGravity == true)
        {
            rig.velocity = new Vector2(0, 0);
            rig.angularVelocity = new Vector3(0,0,0);
            rig.useGravity = false;
        }
        transform.Find("start").GetComponent<SphereCollider>().isTrigger = true;
        transform.Find("end").GetComponent<SphereCollider>().isTrigger = true;
        transform.Find("body").GetComponent<BoxCollider>().isTrigger = true;
        //回到初始点
        transform.rotation = initdata.initRo;
        transform.position = initdata.initPos;
    }
    private void OnDestroy()
    {
        PlayButton.startPlay.TurnOn -= StartWork;
        PlayButton.startPlay.TurnOff -= EndWork;
    }
    #endregion

    #region 画线时重叠数更新
    private void OnTriggerEnter(Collider collision)
    {
        if (isdrawing)
        {
            if (collision.tag != "OutputArea")
            {

                colnum++;
                Debug.Log("碰撞发生" + Time.frameCount + "," + isStart);
            }

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (isdrawing)
        {
            if (collision.tag != "OutputArea")
            {
                colnum--;
                Debug.Log("碰撞退出" + Time.frameCount);
            }
        }
    }
    #endregion
    
    private void BodyUpdata()
    {
        body.SetPositions(new Vector3[2] { start.position, end.position });
    }
    //维护线体的位置
    private void Ondrawupdataline()
    {
        //画线时碰撞体形状的更新与重叠时线的颜色的更新
        if (isdrawing)
        {
            col.position = (start.position + end.position) * 0.5f;
            col.right = end.position - start.position;
            col.GetComponent<BoxCollider>().size = new Vector3((start.position - end.position).magnitude, 1f,1f);
            if (colnum > 0)
            {
                oncollision = true;
                body.startColor=Color.red;
                body.endColor = Color.red;
                start.GetComponent<SpriteRenderer>().color = Color.red;
                end.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                oncollision = false;
                body.startColor = Color.white;
                body.endColor = Color.white;
                start.GetComponent<SpriteRenderer>().color = Color.white;
                end.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //创建锚点
            CreateCir();
        }
        // 结束画线后恢复图层关系
        if (!isdrawing)
        {
            
            if (start.GetComponent<SpriteRenderer>().sortingOrder == 1 || end.GetComponent<SpriteRenderer>().sortingOrder == 1)
            {
                start.GetComponent<SpriteRenderer>().sortingOrder = 0;
                end.GetComponent<SpriteRenderer>().sortingOrder = 0;
                body.sortingOrder = 0;
            }
        }
    }

    //画线时线的碰撞体和图层更新
    private void Onrayupdataline()
    {
        if (GameMode.gameMode != GameMode.gamemode.onDrawing)
        {
            //鼠标落在线上时显示锚点
            if (onRay)
            {
                if ( GameMode.gameMode != GameMode.gamemode.onLink)
                {
                    for (int i = 0; i < cirnum; i++)
                    {
                        cirs[i].gameObject.SetActive(true);
                    }
                }
            }
            //鼠标不在线上时隐藏锚点
            else
            {
                for (int i = 0; i < cirnum; i++)
                {
                    cirs[i].localScale = new Vector3(0.15f, 0.15f, 0);
                    if (!cirs[i].GetComponent<PointUnderMouse>().isPoint)
                    {
                        cirs[i].gameObject.SetActive(false);
                    }
                }
            }
            onRay = false;
        }
    }
    //锚点的显示和隐藏
    private void CreateCir()
    {
        var distance = (start.position - end.position).magnitude;
        cirs[0].position = new Vector3(((start.position + end.position) * 0.5f).x, ((start.position + end.position) * 0.5f).y,-1f);
        cirs[1].position = new Vector3(start.position.x, start.position.y, -1f);
        cirs[2].position = new Vector3(end.position.x, end.position.y, -1f);
        cirs[3].position = new Vector3(0.5f * ((start.position + end.position) * 0.5f + start.position).x, 0.5f * ((start.position + end.position) * 0.5f + start.position).y, -1f);
        cirs[4].position = new Vector3(0.5f * ((start.position + end.position) * 0.5f + end.position).x, 0.5f * ((start.position + end.position) * 0.5f + end.position).y, -1f);
        //线的距离少于2时激活一个锚点
        if (distance < 2)
        {
            cirnum = 1;
            for (int i = 0; i < cirs.Count; i++)
            {
                if (i < cirnum)
                {
                    cirs[i].gameObject.SetActive(true);
                }
                else
                {
                    cirs[i].gameObject.SetActive(false);
                }
            }
        }
        //线的距离大于2少于4时激活三个锚点
        if (distance >= 2 && distance <= 4)
        {
            for (int i = 0; i < cirs.Count; i++)
            {
                cirnum = 3;
                if (i < cirnum)
                {
                    cirs[i].gameObject.SetActive(true);
                }
                else
                {
                    cirs[i].gameObject.SetActive(false);
                }
            }
        }
        //线的距离大于4时激活五个锚点
        if (distance > 4)
        {
            for (int i = 0; i < cirs.Count; i++)
            {
                cirnum = 5;
                if (i < cirnum)
                {
                    cirs[i].gameObject.SetActive(true);
                }
                else
                {
                    cirs[i].gameObject.SetActive(false);
                }
            }
        }
    }
    //创建锚点
}
