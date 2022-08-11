using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    public enum gamemode
    {
        onBoard,//非运行状态下鼠标位于空白处（可画线处）
        onDrawing,//画线中
        onLine,//鼠标位于线上
        onLink,//创建连接中
        onPlay,//运行中
        onDrag,//拖拽中
        onRo//旋转中
    }
    public static gamemode gameMode ;
}
