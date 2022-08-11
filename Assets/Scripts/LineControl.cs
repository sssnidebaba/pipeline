using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LineControl : MonoBehaviour
{
    public Transform start,end,col,cirpre;//prefab
    public LineRenderer body;//�ߵ�����
    public List<Transform> cirs;//���ϵ�ê���б�
    public bool isdrawing;//�Ƿ����ڻ���
    public bool oncollision;//���߹������Ƿ��������������ص�
    public bool onRay;//����Ƿ�������
    public bool pointOn;//�Ƿ�����ê��
    public InitData initdata = new InitData();//��ʼ����
    public int colnum = 0;//�ص���������

    [SerializeField]
    private int cirnum=5;//ê������


    bool isStart = false;
    private void Start()
    {
        Debug.Log(Time.frameCount);
        int index = 1;
        //��ʼ��ê������
        for (int i = cirs.Count; i < 5; i++)
        {
            var tempCir = Instantiate(cirpre, (start.position + end.position) * 0.5f, Quaternion.identity, transform);
            cirs.Add(tempCir);
            tempCir.name = index+"";
            index++;
        }
        //����ͼ���ϵ
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

    #region ����������
    private void StartWork(object sender, EventArgs e)
    {
        //������������ײ��״̬
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
        //������������ײ��״̬
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
        //�ص���ʼ��
        transform.rotation = initdata.initRo;
        transform.position = initdata.initPos;
    }
    private void OnDestroy()
    {
        PlayButton.startPlay.TurnOn -= StartWork;
        PlayButton.startPlay.TurnOff -= EndWork;
    }
    #endregion

    #region ����ʱ�ص�������
    private void OnTriggerEnter(Collider collision)
    {
        if (isdrawing)
        {
            if (collision.tag != "OutputArea")
            {

                colnum++;
                Debug.Log("��ײ����" + Time.frameCount + "," + isStart);
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
                Debug.Log("��ײ�˳�" + Time.frameCount);
            }
        }
    }
    #endregion
    
    private void BodyUpdata()
    {
        body.SetPositions(new Vector3[2] { start.position, end.position });
    }
    //ά�������λ��
    private void Ondrawupdataline()
    {
        //����ʱ��ײ����״�ĸ������ص�ʱ�ߵ���ɫ�ĸ���
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
            //����ê��
            CreateCir();
        }
        // �������ߺ�ָ�ͼ���ϵ
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

    //����ʱ�ߵ���ײ���ͼ�����
    private void Onrayupdataline()
    {
        if (GameMode.gameMode != GameMode.gamemode.onDrawing)
        {
            //�����������ʱ��ʾê��
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
            //��겻������ʱ����ê��
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
    //ê�����ʾ������
    private void CreateCir()
    {
        var distance = (start.position - end.position).magnitude;
        cirs[0].position = new Vector3(((start.position + end.position) * 0.5f).x, ((start.position + end.position) * 0.5f).y,-1f);
        cirs[1].position = new Vector3(start.position.x, start.position.y, -1f);
        cirs[2].position = new Vector3(end.position.x, end.position.y, -1f);
        cirs[3].position = new Vector3(0.5f * ((start.position + end.position) * 0.5f + start.position).x, 0.5f * ((start.position + end.position) * 0.5f + start.position).y, -1f);
        cirs[4].position = new Vector3(0.5f * ((start.position + end.position) * 0.5f + end.position).x, 0.5f * ((start.position + end.position) * 0.5f + end.position).y, -1f);
        //�ߵľ�������2ʱ����һ��ê��
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
        //�ߵľ������2����4ʱ��������ê��
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
        //�ߵľ������4ʱ�������ê��
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
    //����ê��
}
