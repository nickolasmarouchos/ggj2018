using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;

    Camera mainCam;
    List<WebNode> nodes;
    bool webBuildMode = false;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        nodes = new List<WebNode>();
    }
	
	// Update is called once per frame
	void Update () {

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0))
        {
            if (CheckWebNodeHit())
            {
                ResolveWebNodeHit();
            }
        }

        if (webBuildMode)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
                Input.GetMouseButtonUp(0))
            {
                CreateNewNode();
                webBuildMode = false;
            }
        }
    }

    private bool CheckWebNodeHit()
    {
        Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //RaycastHit2D hit = Physics2D.Raycast(screenPos, -Vector2.up);
        if (Physics.Raycast(screenPos, Vector3.forward))
        {
            Debug.Log("hit object");
        }

        return true;
    }

    private void ResolveWebNodeHit()
    {
        webBuildMode = true;
    }

    private void CreateNewNode()
    {
        WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);
        Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        node.transform.position = new Vector3(screenPos.x, screenPos.y, 0f);
        node.transform.parent = transform;

        node.Init(this);
        nodes.Add(node);
    }
}
