using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkLine : MonoBehaviour
{
    public PointUnderMouse start, end;
    public ConfigurableJoint phyLink;

    private LineRenderer LR;
    private void Start()
    {
        LR = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (GameMode.gameMode != GameMode.gamemode.onLink)
        {
            if (!start|| !end||!start.isPoint||!end.isPoint)
            {
                Destroy(gameObject);
                Destroy(phyLink);
            }
        }
        if (GameMode.gameMode==GameMode.gamemode.onDrag)
        {
            LR.SetPosition(0,start.transform.position);
            LR.SetPosition(1, end.transform.position);
        }
    }
}
