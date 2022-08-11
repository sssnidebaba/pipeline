using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PointUnderMouse : MonoBehaviour
{
    public Sprite cir0,cir1;//�Ƿ����õ�ͼƬ
    public Transform preLink;//����prefab
    public bool isPoint;//�Ƿ�����
    public bool underMouse;//�Ƿ�λ�������

    private float roSpeed;//��ת�ٶ�
    private float changeScaleSpeed;//�Ŵ���С���������ٶ�

    private void Start()
    {
        roSpeed = 50f;
        changeScaleSpeed = 0.001f;
    }
    private void Update()
    {
        Ontouch();
    }
    private void Ontouch()
    {
        if (underMouse)
        {
            transform.localScale = new Vector3(Mathf.MoveTowards(transform.localScale.x, 0.2f, changeScaleSpeed), Mathf.MoveTowards(transform.localScale.y, 0.2f, changeScaleSpeed), 0);
            transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * roSpeed);
        }
        else
        {   
            transform.localScale = new Vector3(Mathf.MoveTowards(transform.localScale.x, 0.15f, changeScaleSpeed), Mathf.MoveTowards(transform.localScale.y, 0.15f, changeScaleSpeed), 0);
        }
        underMouse = false;
    }//�Ŵ���С����
    public void Add()
    {
            var ls = transform.parent.GetComponent<LineControl>();
            ls.pointOn = true;
            foreach (var tmp in ls.cirs)
            {
                tmp.GetComponent<SpriteRenderer>().sprite = cir0;
                tmp.GetComponent<PointUnderMouse>().isPoint = false;
            }

        if (transform.parent.GetComponent<HingeJoint>() == null)
            transform.parent.gameObject.AddComponent<HingeJoint>();

        var temp = transform.parent.GetComponent<HingeJoint>();
        temp.anchor= transform.localPosition;
       
        transform.GetComponent<SpriteRenderer>().sprite = cir1;
            isPoint = true;

    }//����ê��
    public void Remove()//�ر�ê���ɾ����
    {
        if (isPoint)
        {
            transform.parent.GetComponent<LineControl>().pointOn=false;
            isPoint = false;
            transform.GetComponent<SpriteRenderer>().sprite = cir0;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("cir"+transform.name+"ײ"+other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("cir"+transform.name+"ײ"+other.name);
    }
}
