using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MouseManager : MonoBehaviour
{
    public static MouseManager mouseManager;

    private Collider hasSomething;//������ڵ���ײ����ײ��
    private Vector3 drawOffset;
    
    public GameMode.gamemode ls;//�����Ϸģʽ

    private void Awake()
    {
        mouseManager = this;
    }

    private void Update()
    {
        ls = GameMode.gameMode;
        mouseControl();
    }
    private  Collider mousepointCol()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider;
        }
        else return null;

    }
    //����������ڵ���ײ����ײ��
    private void mouseControl()
    {
        //if (mousepointCol() != null)
        //{
        //    Debug.Log(mousepointCol().name);
        //}
        //else
        //{
        //    Debug.Log("null");
        //}
        //���������ڵ���ײ����ײ��

        //����ڿհ״�->���������
        if (GameMode.gameMode == GameMode.gamemode.onBoard)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                hasSomething = mousepointCol();
                if (hasSomething != null)
                {
                    if (hasSomething.transform.tag == "Line")
                    {
                        hasSomething.transform.parent.GetComponent<LineControl>().onRay = true;
                        GameMode.gameMode = GameMode.gamemode.onLine;
                    }
                    else if (hasSomething.transform.tag == "Point")
                    {
                        hasSomething.transform.parent.GetComponent<LineControl>().onRay = true;
                        GameMode.gameMode = GameMode.gamemode.onLine;
                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                        DrawlineManager.drawlineManager.startDraw = true;
                }
            }
        }

        //��������ϵ��������ê������ӵ� �� �Ҽ�ȥ��ê�����
        else if (GameMode.gameMode == GameMode.gamemode.onLine)
        {
            hasSomething = mousepointCol();


            if (hasSomething == null)
            {
                GameMode.gameMode = GameMode.gamemode.onBoard;
            }//�뿪�߸ı�gamemode
            else if (hasSomething.transform.tag == "Line")
            {
                hasSomething.transform.parent.GetComponent<LineControl>().onRay = true;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Destroy(hasSomething.transform.parent.GetComponent<HingeJoint>());
                    drawOffset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)) - hasSomething.transform.parent.position;
                    GameMode.gameMode = GameMode.gamemode.onDrag;
                }
                
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    Destroy(hasSomething.transform.parent.gameObject);
                }//ɾ���߶�

            }

            else if (hasSomething.transform.tag == "Point")
            {
                var temp = hasSomething.GetComponent<PointUnderMouse>();
                temp.transform.parent.GetComponent<LineControl>().onRay = true;
                temp.underMouse = true;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (temp.isPoint)
                    {
                        CreateLinkManager.createLinkManager.CreateLink(temp.transform.position, temp.GetComponent<PointUnderMouse>());
                        GameMode.gameMode = GameMode.gamemode.onLink;
                    }
                    else
                    {
                        temp.Add();
                    }
                }//������ӵ�
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    temp.Remove();
                }//ɾ�����ӵ�
            }
            else if (hasSomething.transform.tag == "Line")
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Destroy(hasSomething.transform.parent.gameObject);
                }
            }
        }
        //��ק����
        else if (GameMode.gameMode == GameMode.gamemode.onDrag)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                hasSomething.transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)) - drawOffset;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                hasSomething.transform.parent.gameObject.AddComponent<HingeJoint>();
                GameMode.gameMode = GameMode.gamemode.onLine;
                hasSomething.transform.parent.GetComponent<LineControl>().initdata.initPos = hasSomething.transform.parent.position;
            }
        }

        //����ʱ����������ߺ��Ҽ�ȥ����
        else if (GameMode.gameMode == GameMode.gamemode.onDrawing)
        {
            hasSomething = mousepointCol();
            
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (hasSomething.transform.parent.GetComponent<LineControl>().colnum == 0)
                    DrawlineManager.drawlineManager.endDraw = true;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                DrawlineManager.drawlineManager.deleteDraw = true;
            }
        }
        else if (GameMode.gameMode == GameMode.gamemode.onLink)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CreateLinkManager.createLinkManager.CancelLink();
            }
            hasSomething = mousepointCol();
            if (hasSomething)
            {
                if (hasSomething.transform.tag == "Point")
                {
                    var tmp = hasSomething.GetComponent<PointUnderMouse>();
                    tmp.underMouse = true;
                    tmp.transform.parent.GetComponent<LineControl>().onRay = true;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        CreateLinkManager.createLinkManager.FinishLink(tmp.transform.position, tmp.GetComponent<PointUnderMouse>());
                    }
                }
            }
        }
    }
    //����������
} 