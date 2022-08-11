using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawlineManager : MonoBehaviour
{
    public static DrawlineManager drawlineManager;

    public bool startDraw,deleteDraw,endDraw;//开始画线，删除线，结束画线（一次性）

    [SerializeField]
    private bool isdrawing;//是否处于画线
    [SerializeField]
    private GameObject preline;//线prefab
    private Transform  end;//结束点

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
            }//开始画线
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
                }//画完
                if (deleteDraw)
                {
                    GameMode.gameMode = GameMode.gamemode.onBoard;
                    isdrawing = false;
                    Destroy(end.parent.gameObject);
                    deleteDraw = false;
                }//删除
            }//结束画线
        }
    }
}
