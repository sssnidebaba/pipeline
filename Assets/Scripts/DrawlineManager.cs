using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawlineManager : MonoBehaviour
{
    public static DrawlineManager drawlineManager;

    public bool startDraw,deleteDraw,endDraw;//��ʼ���ߣ�ɾ���ߣ��������ߣ�һ���ԣ�

    [SerializeField]
    private bool isdrawing;//�Ƿ��ڻ���
    [SerializeField]
    private GameObject preline;//��prefab
    private Transform  end;//������

    private void Awake()
    {
        drawlineManager = this;
    }
    private void Update()
    {
        Drawline();
    }
    private void Drawline()
    {
        if (GameMode.gameMode == GameMode.gamemode.onBoard)
        {
            if (!isdrawing)
            {
                if (startDraw)
                {
                    GameMode.gameMode = GameMode.gamemode.onDrawing;
                    isdrawing = true;
                    var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    end = Instantiate(preline, new Vector3(pos.x, pos.y, 0f), Quaternion.identity).transform.Find("end");
                    end.parent.GetComponent<LineControl>().isdrawing = true;
                    startDraw = false;
                }
            }//��ʼ����
        }
        else if (GameMode.gameMode == GameMode.gamemode.onDrawing)
        {
            if (isdrawing)
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                end.transform.position = new Vector3(pos.x, pos.y, 0f);
                if (endDraw)
                {
                    if (!end.parent.GetComponent<LineControl>().oncollision)
                    {
                        GameMode.gameMode = GameMode.gamemode.onBoard;
                        isdrawing = false;
                        end.parent.GetComponent<LineControl>().isdrawing = false;
                        endDraw = false;
                    }
                }//����
                if (deleteDraw)
                {
                    GameMode.gameMode = GameMode.gamemode.onBoard;
                    isdrawing = false;
                    Destroy(end.parent.gameObject);
                    deleteDraw = false;
                }//ɾ��
            }//��������
        }
    }
}
