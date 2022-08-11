using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PointUnderMouse : MonoBehaviour
{
    public Sprite cir0,cir1;//是否启用的图片
    public Transform preLink;//连接prefab
    public bool isPoint;//是否启用
    public bool underMouse;//是否位于鼠标下

    private float roSpeed;//旋转速度
    private float changeScaleSpeed;//放大缩小动画运行速度

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
    }//放大缩小动画
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

    }//启动锚点
    public void Remove()//关闭锚点或删除线
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
        Debug.Log("cir"+transform.name+"撞"+other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("cir"+transform.name+"撞"+other.name);
    }
}
