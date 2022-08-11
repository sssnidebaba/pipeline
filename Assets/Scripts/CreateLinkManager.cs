using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLinkManager : MonoBehaviour
{
    public static CreateLinkManager createLinkManager;
    public LineRenderer preLinkLine;//¡¨Ω”œﬂprefab

    private LineRenderer curLinkLine;
    private Transform start, end;
    private void Awake()
    {
        createLinkManager = this;
    }
    private void Update()
    {
        OnLink();
    }
    public void CreateLink(Vector2 startPos,PointUnderMouse ls)
    {
        start = ls.transform;
        curLinkLine= Instantiate(preLinkLine);
        curLinkLine.GetComponent<LinkLine>().start = ls;
        curLinkLine.SetPosition(0, startPos);
        curLinkLine.SetPosition(1, startPos);
    }
    public void FinishLink(Vector2 endPos, PointUnderMouse ls)
    {
        end = ls.transform;
        var tmp= start.parent.gameObject.AddComponent<ConfigurableJoint>();
        tmp.autoConfigureConnectedAnchor = false;
        tmp.connectedBody = end.parent.GetComponent<Rigidbody>();
        tmp.enableCollision = true;
        tmp.anchor = start.transform.localPosition;
        tmp.connectedAnchor = end.transform.localPosition;
        tmp.angularZMotion = ConfigurableJointMotion.Locked;

        curLinkLine.GetComponent<LinkLine>().phyLink = tmp;

        curLinkLine.GetComponent<LinkLine>().end = ls;
        curLinkLine.SetPosition(1, endPos);
        GameMode.gameMode = GameMode.gamemode.onLine;
        curLinkLine = null;
    }
    public void CancelLink()
    {
        Destroy(curLinkLine.gameObject);
        GameMode.gameMode = GameMode.gamemode.onBoard;
    }
    private void OnLink()
    {
        if (GameMode.gameMode == GameMode.gamemode.onLink)
        {
            curLinkLine.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10f)));
        }
    }
}
